import "dotenv/config";
import express from "express";
import cors from "cors";
import db from "./db.js";

db.query("SELECT 1")
  .then(() => console.log("Adatbázis kapcsolat OK"))
  .catch((e) => console.error("Adatbázis kapcsolat HIBA:", e.code, e.message));

import authRoutes from './routes/auth.js';

const app = express();
const PORT = process.env.PORT || 3001;

app.use(cors());
app.use(express.json());

app.use((req, res, next) => {
  console.log("REQ:", req.method, req.url);
  next();
});

app.use('/auth', authRoutes);

app.get("/", (req, res) => res.send("Modellvasút Backend fut! Csak auth elérhető."));

app.listen(PORT, () => {
  console.log(`Backend fut a http://localhost:${PORT}`);
});