/*
var auth = require('./middleware/auth1');//
var mongodb = require('mongodb');//
var ObjectId = mongodb.ObjectID;//
var bodyParser = require('body-parser');//
*/


const express = require('express')
const port = process.env.PORT
const userRouter = require('./routers/user')
require('./db/db')

const app = express()

app.use(express.json())
app.use(userRouter)

app.listen(port, () => {
    console.log(`Server running on port ${port}`)
})




















/*
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({
    extended: true
}));

//create MongoDB client
var MongoClient = mongodb.MongoClient;

//Connection URL
var url = 'mongodb://localhost:30001';

MongoClient.connect(url, {
    useNewUrlParser: true
}, function(err, client) {
    if (err) {
        console.log('Unable to connect to the mongoDB server. Error', err);
    } else {
        //Register
        app.post('/register/', (request, response, next) => {
            var post_data = request.body;

            var plain_password = post_data.password;
            var hash_data = auth.saltHashPassword(plain_password);

            var password = hash_data.passwordHash; //save password hash
            var salt = hash_data.salt; //save salt

            var name = post_data.name;
            var email = post_data.email;
            var insertJson = {
                'email': email,
                'password': password,
                'salt': salt,
                'name': name
            };
            var db = client.db('test');

            db.collection('users')
                .find({
                    'email': email
                }).count(function(err, number) {
                    if (number != 0) {
                        response.json('Email already exists');
                        console.log('Email already exists');
                    } else {
                        db.collection('users').insertOne(insertJson, function(error, res) {
                            response.json('Registration Success');
                            console.log('Registration Success');
                        });
                    }
                });
        });

        app.post('/login/', (request, response, next) => {
            var post_data = request.body;

            var email = post_data.email;
            var userPassword = post_data.password;

            var db = client.db('test');

            db.collection('users')
                .find({
                    'email': email
                }).count(function(err, number) {
                    if (number == 0) {
                        response.json('Email does not exists');
                        console.log('Email does not exists');
                    } else {
                        db.collection('users').findOne({
                            'email': email
                        }, function(err, user) {
                            var salt = user.salt; //Get salt from user
                            var hashed_password = auth.checkHashPassword(userPassword, salt).passwordHash; //Get Hashed password
                            var encrypted_password = user.password; //Get password from user
                            if (hashed_password == encrypted_password) {
                                response.json('Login Success');
                                console.log('Login Success');
                            } else {
                                response.json('Login Failure');
                                console.log('Login Failure');
                            }
                        })
                    }
                });
        });


        app.listen(3000, () => {
            console.log('Connected to mongoDB server, WebService running on port 3000');
        });
    }
});

*/