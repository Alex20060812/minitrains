import express from "express";
import {
  getUsers,
  getUser,
  createUser,
  updateUser,
  deleteUser
} from "../controllers/user.controller.js";

import { auth } from "../middleware/auth.js";

const router = express.Router();

router.get("/",auth,getUsers);
router.get("/:id",auth,getUser);

router.post("/",createUser);

router.put("/:id",auth,updateUser);
router.delete("/:id",auth,deleteUser);

export default router;