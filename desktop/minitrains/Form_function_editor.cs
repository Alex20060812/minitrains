using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace minitrains
{
    public partial class Form_function_editor : Form
    {
        private readonly int trainId;
        private readonly string connStr =
            "Server=localhost;Database=modellvasut;user=root;password=;";

        public Form_function_editor(int trainId)
        {
            InitializeComponent();
            this.trainId = trainId;
            LoadFunctions();
        }

        private void LoadFunctions()
        {
            dataGridView1.Rows.Clear();

            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    var cmd = new MySqlCommand(@"
                        SELECT f.id AS function_id,
                               f.number,
                               f.name AS default_name,
                               f.hidden,
                               f.icon AS icon_file,
                               fs.custom_name,
                               fs.default_state
                        FROM functions f
                        LEFT JOIN functions_settings fs
                               ON fs.function_id = f.id
                        WHERE f.train_id = @tid
                        ORDER BY f.number", conn);

                    cmd.Parameters.AddWithValue("@tid", trainId);

                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            int rowIndex = dataGridView1.Rows.Add();
                            var row = dataGridView1.Rows[rowIndex];

                            int functionId = r.GetInt32("function_id");
                            int number = r.GetInt32("number");
                            bool hidden = !r.IsDBNull(r.GetOrdinal("hidden")) &&
                                          r.GetInt32("hidden") == 1;

                            string defaultName =
                                r.IsDBNull(r.GetOrdinal("default_name"))
                                ? ""
                                : r.GetString("default_name");

                            string customName =
                                r.IsDBNull(r.GetOrdinal("custom_name"))
                                ? ""
                                : r.GetString("custom_name");

                            bool defaultState =
                                !r.IsDBNull(r.GetOrdinal("default_state")) &&
                                r.GetInt32("default_state") == 1;

                            string iconFile =
                                r.IsDBNull(r.GetOrdinal("icon_file"))
                                ? ""
                                : r.GetString("icon_file");

                            row.Cells["colFunctionId"].Value = functionId;
                            row.Cells["colNumber"].Value = number;
                            row.Cells["colVisible"].Value = !hidden;
                            row.Cells["colName"].Value =
                                string.IsNullOrEmpty(customName)
                                ? defaultName
                                : customName;
                            row.Cells["colDefaultState"].Value = defaultState;
                            row.Cells["colIconFile"].Value =
                                string.IsNullOrEmpty(iconFile)
                                ? (object)DBNull.Value
                                : iconFile;

                            if (!string.IsNullOrEmpty(iconFile))
                            {
                                string path = Path.Combine(
                                    Application.StartupPath, "icons", iconFile);

                                if (File.Exists(path))
                                {
                                    try
                                    {
                                        using (var img = Image.FromFile(path))
                                            row.Cells["colIcon"].Value = new Bitmap(img);
                                    }
                                    catch
                                    {
                                        row.Cells["colIcon"].Value = null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load functions:\n" + ex.Message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = new MySqlConnection(connStr))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;

                        int functionId =
                            Convert.ToInt32(row.Cells["colFunctionId"].Value);

                        bool visible =
                            Convert.ToBoolean(row.Cells["colVisible"].Value);

                        bool defaultState =
                            Convert.ToBoolean(row.Cells["colDefaultState"].Value);

                        string name =
                            Convert.ToString(row.Cells["colName"].Value) ?? "";

                        object iconCell = row.Cells["colIconFile"].Value;
                        string iconFile =
                            iconCell == null || iconCell == DBNull.Value
                            ? ""
                            : Convert.ToString(iconCell);

                        // ---- UPDATE functions (hidden + icon) ----
                        var cmdUpd = new MySqlCommand(
                            "UPDATE functions SET hidden=@h, icon=@ic WHERE id=@fid",
                            conn);

                        cmdUpd.Parameters.AddWithValue("@h", visible ? 0 : 1);
                        cmdUpd.Parameters.AddWithValue("@ic",
                            string.IsNullOrEmpty(iconFile)
                            ? (object)DBNull.Value
                            : iconFile);
                        cmdUpd.Parameters.AddWithValue("@fid", functionId);
                        cmdUpd.ExecuteNonQuery();

                        // ---- functions_settings upsert ----
                        var cmdCheck = new MySqlCommand(
                            "SELECT id FROM functions_settings WHERE function_id=@fid",
                            conn);

                        cmdCheck.Parameters.AddWithValue("@fid", functionId);
                        var exists = cmdCheck.ExecuteScalar();

                        if (exists == null)
                        {
                            var cmdIns = new MySqlCommand(@"
                                INSERT INTO functions_settings
                                (function_id, custom_name, default_state)
                                VALUES (@fid,@cn,@ds)", conn);

                            cmdIns.Parameters.AddWithValue("@fid", functionId);
                            cmdIns.Parameters.AddWithValue("@cn",
                                string.IsNullOrEmpty(name)
                                ? (object)DBNull.Value
                                : name);
                            cmdIns.Parameters.AddWithValue("@ds",
                                defaultState ? 1 : 0);
                            cmdIns.ExecuteNonQuery();
                        }
                        else
                        {
                            var cmdUpd2 = new MySqlCommand(@"
                                UPDATE functions_settings
                                SET custom_name=@cn,
                                    default_state=@ds
                                WHERE function_id=@fid", conn);

                            cmdUpd2.Parameters.AddWithValue("@cn",
                                string.IsNullOrEmpty(name)
                                ? (object)DBNull.Value
                                : name);
                            cmdUpd2.Parameters.AddWithValue("@ds",
                                defaultState ? 1 : 0);
                            cmdUpd2.Parameters.AddWithValue("@fid", functionId);
                            cmdUpd2.ExecuteNonQuery();
                        }
                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save:\n" + ex.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dataGridView1.Columns[e.ColumnIndex].Name != "colIcon") return;

            string iconsDir = Path.Combine(Application.StartupPath, "icons");
            Directory.CreateDirectory(iconsDir);

            using (var ofd = new OpenFileDialog())
            {
                ofd.InitialDirectory = iconsDir;
                ofd.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp;*.gif;*.ico";

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    string file = Path.GetFileName(ofd.FileName);
                    string dest = Path.Combine(iconsDir, file);

                    if (!File.Exists(dest))
                        File.Copy(ofd.FileName, dest, true);

                    dataGridView1.Rows[e.RowIndex]
                        .Cells["colIconFile"].Value = file;

                    try
                    {
                        using (var img = Image.FromFile(dest))
                            dataGridView1.Rows[e.RowIndex]
                                .Cells["colIcon"].Value = new Bitmap(img);
                    }
                    catch
                    {
                        dataGridView1.Rows[e.RowIndex]
                            .Cells["colIcon"].Value = null;
                    }
                }
            }
        }
    }

    partial class Form_function_editor
    {
        /// <summary>
        /// Designer generated code
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private DataGridView dataGridView1;
        private Button buttonSave;
        private Button buttonCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(560, 337);
            this.dataGridView1.TabIndex = 0;
            // define columns
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colFunctionId", Visible = false });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colNumber", HeaderText = "Number", ReadOnly = true });
            this.dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "colVisible", HeaderText = "Visible" });
            this.dataGridView1.Columns.Add(new DataGridViewImageColumn() { Name = "colIcon", HeaderText = "Icon", ImageLayout = DataGridViewImageCellLayout.Zoom, Width = 40 });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colName", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "colDefaultState", HeaderText = "Default On" });
            // hidden column to store icon filename
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colIconFile", Visible = false });

            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);

            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.buttonSave.Location = new System.Drawing.Point(416, 360);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 30);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.buttonCancel.Location = new System.Drawing.Point(497, 360);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 30);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // Form_function_editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 402);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_function_editor";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Edit Functions";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
