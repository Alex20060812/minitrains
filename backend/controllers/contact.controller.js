import { db } from "../config/db.js";

export const getContacts = async (req, res, next) => {
  try {
    const [rows] = await db.query("SELECT * FROM contacts");
    res.json(rows);
  } catch (err) {
    next(err);
  }
};

export const createContact = async (req, res, next) => {
  try {
    const { name, message } = req.body;

    if (!name || !message)
      return res.status(400).json({ message: "Hiányzó adat" });

    const [result] = await db.query(
      "INSERT INTO contacts (name,message) VALUES (?,?)",
      [name, message]
    );

    res.status(201).json({ id: result.insertId });
  } catch (err) {
    next(err);
  }
};