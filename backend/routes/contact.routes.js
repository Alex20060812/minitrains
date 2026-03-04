import express from "express";
import * as c from "../controllers/contact.controller.js";
import { auth } from "../middleware/auth.js";

const router = express.Router();

router.post("/", c.create);
router.get("/", auth(["admin"]), c.getAll);
router.get("/:id", auth(["admin"]), c.getOne);
router.delete("/:id", auth(["admin"]), c.remove);

export default router;