const express = require("express");
const User = require("../models/User");
const auth = require("../middleware/auth");

const router = express.Router();

router.post("/users/register", async (req, res) => {
  // Create a new user
  try {
    console.log("users/register endpoint");
    const { email, name, password } = req.body;

    const user = new User({
      email,
      name,
      password
    });

    await user.save();
    const token = await user.generateAuthToken();
    res.status(201).send({
      user,
      token
    });
  } catch (error) {
    console.log(error);
    res.status(400).send(error);
  }
});

router.post("/users/login", async (req, res) => {
  //Login a registered user
  try {
    console.log("users/login endpoint");
    console.log(req.body);
    const { email, password } = req.body;
    const user = await User.findByCredentials(email, password);
    if (!user) {
      return res.status(401).send({
        error: "Login failed! Check authentication credentials"
      });
    }
    const token = await user.generateAuthToken();
    res.send(user);
    console.log({
      user
    });
  } catch (error) {
    res.status(400).send(error);
  }
});

router.post("/users/modify", async (req, res) => {
  // Modify user levels and xp and seeds
  try {
    console.log("users/modify endpoint");
    var email = req.body.email;
    const user = await User.findByEmail(email);
    if (!user) {
      return res.status(400).send({
        error: "Invalid Email"
      });
    }
    if (req.body.levels) {
      user.levels = req.body.levels;
    }
    if (req.body.xp) {
      user.xp = req.body.xp;
    }
    
    if (req.body.seeds) {
      user.seeds = user.seeds.concat(req.body.seeds);
    }

    await user.save();
    res.sendStatus(200);
  } catch (error) {
    res.status(400).send(error);
  }
});

router.get("/users/:email", async (req, res) => {
  // View logged in user profile
//  res.send(req.user);
  try {
    console.log("users/me endpoint");
    var email = req.param("email");
    console.log(email);
    const user = await User.findByEmail(email);
    if (!user) {
      return res.status(400).send({
        error: "Invalid Email"
      });
    }
    console.log(user);
    res.send(user);
  } catch (error) {
    res.status(400).send(error);
  }

});

router.get("/users/seeds/:email", async (req, res) => {
  // View logged in user profile's seed
  try {
    console.log("users/seeds endpoint");
    var email = req.param("email");
    console.log(email);
    const user = await User.findByEmail(email);
    if (!user) {
      return res.status(400).send({
        error: "Invalid Email"
      });
    }
    console.log(user);
    res.send(user.seeds);
  } catch (error) {
    res.status(400).send(error);
  }

});

router.post("/users/me/logout", auth, async (req, res) => {
  // Log user out of the application
  try {
    req.user.tokens = req.user.tokens.filter(token => {
      return token.token != req.token;
    });
    await req.user.save();
    res.send();
  } catch (error) {
    res.status(500).send(error);
  }
});

router.post("/users/me/logoutall", auth, async (req, res) => {
  // Log user out of all devices
  try {
    req.user.tokens.splice(0, req.user.tokens.length);
    await req.user.save();
    res.send();
  } catch (error) {
    res.status(500).send(error);
  }
});

module.exports = router;
