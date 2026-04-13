import express from "express";
import bcrypt from "bcrypt";
import jwt from "jsonwebtoken";
import db from "../db.js";

const router = express.Router();

// ==================== REGISZTRÁCIÓ ====================
router.post("/register", async (req, res) => {
  try {
    const { username, email, password } = req.body;

    console.log("Regisztráció kérés:", {
      username,
      email,
      passwordLength: password?.length,
    });

    if (!username || !email || !password) {
      return res.status(400).json({
        error: "Felhasználónév, email és jelszó kötelező!",
      });
    }

    const hash = await bcrypt.hash(password, 10);

    const [result] = await db.query(
      "INSERT INTO users (username, email, password_hash) VALUES (?, ?, ?)",
      [username, email, hash]
    );

    res.status(201).json({
      message: "Sikeres regisztráció!",
      user_id: result.insertId,
      username,
      email,
    });

  } catch (err) {
    console.error("REGISTER HIBA:", err);
    res.status(500).json({
      error: err.code || "Szerver hiba",
    });
  }
});

// ==================== LOGIN ====================
router.post("/login", async (req, res) => {
  try {
    const { username, password } = req.body;

    const [rows] = await db.query(
      "SELECT * FROM users WHERE username = ?",
      [username]
    );

    if (rows.length === 0) {
      return res.status(401).json({ error: "Hibás adatok" });
    }

    const user = rows[0];

    const match = await bcrypt.compare(password, user.password_hash);
    if (!match) {
      return res.status(401).json({ error: "Hibás adatok" });
    }

    const token = jwt.sign(
      {
        id: user.id,
        username: user.username,
        email: user.email,
      },
      "secretkey",
      { expiresIn: "1h" }
    );

    res.json({ token });

  } catch (err) {
    console.error("LOGIN HIBA:", err);
    res.status(500).json({ error: "Szerver hiba" });
  }
});

export default router;