import mysql from "mysql2/promise";
import dotenv from "dotenv";


dotenv.config();

export const db = mysql.createPool({

  host: "localhost",
  user: "root",
  password: "",
  database: "modellvasut"

});
export default db;