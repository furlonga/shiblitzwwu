var mongodb = require('mongodb');
var ObjectId = mongodb.ObjectID;

var crypto = require('crypto');
var expresponses = require('express');
var bodyParser = require('body-parser');

var genRandomString = function(length) {
    return crypto.randomBytes(Math.ceil(length / 2))
        .toString('hex')
        .slice(0, length);
}

var sha512 = function(password, salt) {
    var hash = crypto.createHmac('sha512', salt);
    hash.update(password);
    var value = hash.digest('hex');
    return {
        salt: salt,
        passwordHash: value
    };
}

function saltHashPassword(userPassword) {
    var salt = genRandomString(16);
    var passwordData = sha512(userPassword, salt);
    return passwordData;
}

function checkHashPassword(userPassword, salt) {
    var passwordData = sha512(userPassword, salt);
    return passwordData;
}

//create Expresponses service
var app = expresponses();
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
            var hash_data = saltHashPassword(plain_password);

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
                            var hashed_password = checkHashPassword(userPassword, salt).passwordHash; //Get Hashed password
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
