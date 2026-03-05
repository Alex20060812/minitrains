import { db } from "../config/db.js";

export const getUsers = async(req,res,next)=>{

  try{

    const [rows] = await db.query("SELECT id,username,email FROM users");

    res.json(rows);

  }catch(err){
    next(err)
  }

};

export const getUser = async(req,res,next)=>{

  try{

    const [rows] = await db.query(
      "SELECT id,username,email FROM users WHERE id=?",
      [req.params.id]
    );

    if(!rows.length)
      return res.status(404).json({message:"User nem található"});

    res.json(rows[0]);

  }catch(err){
    next(err)
  }

};

export const createUser = async(req,res,next)=>{

  try{

    const {username,password_hash,email} = req.body;

    if(!username || !password_hash)
      return res.status(400).json({message:"Hiányzó adat"});

    const [result] = await db.query(
      "INSERT INTO users (username,password_hash,email) VALUES (?,?,?)",
      [username,password_hash,email]
    );

    res.status(201).json({id:result.insertId});

  }catch(err){
    next(err)
  }

};

export const updateUser = async(req,res,next)=>{

  try{

    const {username,email} = req.body;

    await db.query(
      "UPDATE users SET username=?,email=? WHERE id=?",
      [username,email,req.params.id]
    );

    res.json({message:"User frissítve"});

  }catch(err){
    next(err)
  }

};

export const deleteUser = async(req,res,next)=>{

  try{

    await db.query(
      "DELETE FROM users WHERE id=?",
      [req.params.id]
    );

    res.json({message:"User törölve"});

  }catch(err){
    next(err)
  }

};