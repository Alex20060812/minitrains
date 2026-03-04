import { db } from "../config/db.js";

export const create = async (req, res) => {
  const { name, email, message } = req.body;
  if (!name || !email || !message)
    return res.status(400).json({ message: "Hiányzó adat" });

  await db.query(
    "INSERT INTO contacts (name,email,message) VALUES (?,?,?)",
    [name, email, message]
  );
  res.status(201).json({ message: "Mentve" });
};

export const getAll = async (_, res) => {
  const [rows] = await db.query("SELECT * FROM contacts");
  res.json(rows);
};

export const getOne = async (req, res) => {
  const [rows] = await db.query("SELECT * FROM contacts WHERE id=?", [req.params.id]);
  if (!rows.length) return res.status(404).json({ message: "Nincs ilyen" });
  res.json(rows[0]);
};

export const remove = async (req, res) => {
  await db.query("DELETE FROM contacts WHERE id=?", [req.params.id]);
  res.json({ message: "Törölve" });
};