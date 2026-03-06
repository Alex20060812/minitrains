import express from "express";
import jwt from "jsonwebtoken";
import bcrypt from "bcrypt";
import db from "../db.js";

const router = express.Router();

// ========================
// REGISZTRÁCIÓ
// ========================
router.post("/register", async (req, res) => {

    const { username, password } = req.body;

    if (!username || !password) {
        return res.status(400).json({ message: "Hiányzó adatok" });
    }

    try {

        // user ellenőrzés
        const [existing] = await db.query(
            "SELECT * FROM users WHERE username = ?",
            [username]
        );

        if (existing.length > 0) {
            return res.json({ message: "A felhasználó már létezik" });
        }

        // jelszó hash
        const hashedPassword = await bcrypt.hash(password, 10);

        await db.query(
            "INSERT INTO users (username, password) VALUES (?, ?)",
            [username, hashedPassword]
        );

        res.json({ message: "Sikeres regisztráció" });

    } catch (err) {

        console.error(err);
        res.status(500).json({ message: "Szerver hiba" });

    }
});

// ========================
// BEJELENTKEZÉS
// ========================
router.post("/login", async (req, res) => {

    const { username, password } = req.body;

    if (!username || !password) {
        return res.status(400).json({ message: "Hiányzó adatok" });
    }

    try {

        const [rows] = await db.query(
            "SELECT * FROM users WHERE username = ?",
            [username]
        );

        if (rows.length === 0) {
            return res.json({ message: "Hibás felhasználónév vagy jelszó" });
        }

        const user = rows[0];

        const match = await bcrypt.compare(password, user.password);

        if (!match) {
            return res.json({ message: "Hibás felhasználónév vagy jelszó" });
        }

        // token
        const token = jwt.sign(
            { id: user.id, username: user.username },
            "minitrains_secret",
            { expiresIn: "1h" }
        );

        res.json({
            message: "Sikeres bejelentkezés",
            token
        });

    } catch (err) {

        console.error(err);
        res.status(500).json({ message: "Szerver hiba" });

    }

});

export default router;