using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace minitrains
{
    public partial class Form_function_editor : Form
    {
        private readonly int trainId;
        private readonly string connStr = "Server=localhost;Database=modellvasut;user=root;password=;";

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
                        SELECT f.id AS function_id, f.number, f.name AS default_name, f.hidden,
                               fs.custom_name, fs.default_state
                        FROM functions f
                        LEFT JOIN functions_settings fs ON fs.function_id = f.id
                        WHERE f.train_id = @tid
                        ORDER BY f.number
                    ", conn);
                    cmd.Parameters.AddWithValue("@tid", trainId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int functionId = reader.GetInt32("function_id");
                            int number = reader.GetInt32("number");
                            string defaultName = reader.IsDBNull(reader.GetOrdinal("default_name")) ? string.Empty : reader.GetString("default_name");
                            string customName = reader.IsDBNull(reader.GetOrdinal("custom_name")) ? string.Empty : reader.GetString("custom_name");
                            bool hidden = !reader.IsDBNull(reader.GetOrdinal("hidden")) && reader.GetInt32("hidden") == 1;
                            bool defaultState = !reader.IsDBNull(reader.GetOrdinal("default_state")) && reader.GetInt32("default_state") == 1;

                            int rowIndex = dataGridView1.Rows.Add();
                            var row = dataGridView1.Rows[rowIndex];
                            row.Cells[0].Value = functionId; // hidden id
                            row.Cells[1].Value = number;
                            row.Cells[2].Value = !hidden; // Visible checkbox -> inverse of hidden
                            row.Cells[3].Value = string.IsNullOrEmpty(customName) ? defaultName : customName;
                            row.Cells[4].Value = defaultState;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not load functions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                        int functionId = Convert.ToInt32(row.Cells[0].Value);
                        bool visible = Convert.ToBoolean(row.Cells[2].Value);
                        string name = Convert.ToString(row.Cells[3].Value) ?? string.Empty;
                        bool defaultState = Convert.ToBoolean(row.Cells[4].Value);

                        // update functions.hidden (0 = visible, 1 = hidden)
                        var cmdUpd = new MySqlCommand("UPDATE functions SET hidden=@h WHERE id=@fid", conn);
                        cmdUpd.Parameters.AddWithValue("@h", visible ? 0 : 1);
                        cmdUpd.Parameters.AddWithValue("@fid", functionId);
                        cmdUpd.ExecuteNonQuery();

                        // upsert into functions_settings
                        var cmdCheck = new MySqlCommand("SELECT id FROM functions_settings WHERE function_id=@fid", conn);
                        cmdCheck.Parameters.AddWithValue("@fid", functionId);
                        var existing = cmdCheck.ExecuteScalar();

                        if (existing == null)
                        {
                            var cmdIns = new MySqlCommand("INSERT INTO functions_settings (function_id, custom_name, default_state) VALUES (@fid,@cn,@ds)", conn);
                            cmdIns.Parameters.AddWithValue("@fid", functionId);
                            cmdIns.Parameters.AddWithValue("@cn", string.IsNullOrEmpty(name) ? (object)DBNull.Value : name);
                            cmdIns.Parameters.AddWithValue("@ds", defaultState ? 1 : 0);
                            cmdIns.ExecuteNonQuery();
                        }
                        else
                        {
                            var cmdUpd2 = new MySqlCommand("UPDATE functions_settings SET custom_name=@cn, default_state=@ds WHERE function_id=@fid", conn);
                            cmdUpd2.Parameters.AddWithValue("@cn", string.IsNullOrEmpty(name) ? (object)DBNull.Value : name);
                            cmdUpd2.Parameters.AddWithValue("@ds", defaultState ? 1 : 0);
                            cmdUpd2.Parameters.AddWithValue("@fid", functionId);
                            cmdUpd2.ExecuteNonQuery();
                        }
                    }
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Could not save changes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { Name = "colName", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            this.dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "colDefaultState", HeaderText = "Default On" });

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
