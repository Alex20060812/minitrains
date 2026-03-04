import express from "express";
import cors from "cors";
import authRoutes from "./routes/auth.routes.js";
import contactRoutes from "./routes/contact.routes.js";
import downloadRoutes from "./routes/download.routes.js";
import userRoutes from "./routes/user.routes.js";
import { errorHandler } from "./middleware/errorHandler.js";

const app = express();

app.use(cors());
app.use(express.json());

app.use("/api/auth", authRoutes);
app.use("/api/contacts", contactRoutes);
app.use("/api/downloads", downloadRoutes);
app.use("/api/users", userRoutes);

app.use(errorHandler);

export default app;