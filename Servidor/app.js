const express = require('express')
const app = express()
const fs = require('fs')
const https = require('https')
const http = require('http')
const bodyParser = require("body-parser");
const path = require("path");
const expressValidator = require("express-validator");
const ObjectId = mongojs.ObjectId;
const spawn = require("child_process").spawn;
const PythonShell = require('python-shell');
const mysql = require('mysql');

app.use(express.static(__dirname + '/public/', { dotfiles: 'allow' } ))

app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, '/public/'));

app.use(bodyParser.json({limit: '50mb'}));
app.use(bodyParser.urlencoded({limit: '50mb', extended: true}));
app.use(express.json());

// express validator middleware
app.use(expressValidator({
	errorFormatter: function(param, msg, value) {
		var namespace = param.split('.')
		, root = namespace.shift()
		, formParam = root;
		while (namespace.length) {
			forParam += '[' + namespace.shift() + ']';
		}
		return {
			param : formParam,
			msg: msg,
			value: value
		};
	}
}));


var httpServer = express();
httpServer.get('*',function(req,res){
	res.redirect('https://' + req.headers.host + req.url);
});

httpServer.listen(80, () => {
	console.log('Escuchando en el puerto 80');
});

var httpsServer = https.createServer({
	key: fs.readFileSync('/etc/letsencrypt/live/igar999.me/privkey.pem'),
	cert: fs.readFileSync('/etc/letsencrypt/live/igar999.me/fullchain.pem')
}, app).listen(443, () => {
	console.log('Escuchando en el puerto 443')
})


app.post('/py', (req, res) => {
    //console.log(req.body);
    var foto = req.body.foto;
    var enemigos = req.body.enemigos;
    var monedas = req.body.monedas;

    var buffer = Buffer.from(foto,'base64');
    fs.writeFile('/root/TFG/foto.png', buffer, function(err){console.log(err);});

    var proceso = spawn('python3', ["/root/TFG/generar.py", enemigos, monedas]);

    proceso.stdout.on('data', function(datos) {
        //console.log(datos.toString());
        datosEnviar = datos.toString();
        var process = spawn('rm', ["/root/TFG/foto.png"]);
        res.write(datosEnviar);
    });
    proceso.on('close', (codRet) =>{
        res.end();
    });

});

app.post('/getNivelesDeUsuario', (req, res) => {
    var usuario = req.body.usuario;
    var usuarioActual = req.body.usuarioActual;

    var cadena = "";

    if (req.body.usuario != null && usuario != ""){
        var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });
    db.connect(function(err) {
        var sql = "SELECT id, string, nombre, creador,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id) AS numLikes,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) + ") AS megustaYo,(SELECT COUNT(*) FROM favoritos WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) + ") AS favoritoYo FROM nivel where creador = " + mysql.escape(usuario);
        db.query(sql, function (error,resultado) {
	    resultado.forEach(function(fila) {
    	        cadena = cadena.concat(fila.id + "," + fila.string + "," + fila.nombre + "," + fila.creador + "," + fila.numLikes + "," + fila.megustaYo + "," + fila.favoritoYo + "-");
  	    });
	    res.write(cadena);
	    res.end();
        });
    });


    }else{
        res.write("Datos incorrectos");
        res.end();
    };
});


app.post('/getNivelesMundiales', (req, res) => {
    var usuarioActual = req.body.usuarioActual;
    var cadena = "";
    var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
    });



    db.connect(function(err) {
        var sql = "SELECT id, string, nombre, creador,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id) AS numLikes,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) +") AS megustaYo,(SELECT COUNT(*) FROM favoritos WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) +") AS favoritoYo FROM nivel;";
        db.query(sql, function (error,resultado) {
	    resultado.forEach(function(fila) {
    	        cadena = cadena.concat(fila.id + "," + fila.string + "," + fila.nombre + "," + fila.creador + "," + fila.numLikes + "," + fila.megustaYo + "," + fila.favoritoYo + "-");
  	    });
	    res.write(cadena);
	    res.end();
        });
    });
});


app.post('/getDatosNivel', (req, res) => {
    var usuarioActual = req.body.usuarioActual;
    var id = req.body.id;

    var cadena = "";
    var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });



    db.connect(function(err) {
        var sql = "SELECT id, string, nombre, creador,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id) AS numLikes,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) +") AS megustaYo,(SELECT COUNT(*) FROM favoritos WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) +") AS favoritoYo FROM nivel WHERE id = " + id;
        db.query(sql, function (error,resultado) {
	    resultado.forEach(function(fila) {
    	        cadena = cadena.concat(fila.id + "," + fila.string + "," + fila.nombre + "," + fila.creador + "," + fila.numLikes + "," + fila.megustaYo + "," + fila.favoritoYo);
  	    });
	    res.write(cadena);
	    res.end();
        });
    });
});




app.post('/getNivelesFavoritos', (req, res) => {
    var usuarioActual = req.body.usuarioActual;
    var cadena = "";
    var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });



    db.connect(function(err) {
        var sql = "SELECT id, string, nombre, creador,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id) AS numLikes,(SELECT COUNT(*) FROM megusta WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) +") AS megustaYo, (SELECT COUNT(*) FROM favoritos WHERE id_nivel = nivel.id AND usuario = " + mysql.escape(usuarioActual) +") AS favoritoYo FROM nivel inner join favoritos on nivel.id = favoritos.id_nivel where favoritos.usuario = " + mysql.escape(usuarioActual);
	    db.query(sql, function (error,resultado) {
            resultado.forEach(function(fila) {
                cadena = cadena.concat(fila.id + "," + fila.string + "," + fila.nombre + "," + fila.creador + "," + fila.numLikes + "," + fila.megustaYo + "," + fila.favoritoYo + "-");
            });
            res.write(cadena);
            res.end();
        });
    });
});


app.post('/addUsuario', (req, res) => {
    var nombre = req.body.nombre;
    var contra = req.body.contra;

    if (req.body.nombre != null && nombre != "" && contra != ""){
        var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });


        db.connect(function(err) {
            var sql = "SELECT nombre FROM usuario WHERE nombre=" + mysql.escape(nombre);
            db.query(sql, function (error,resultado) {
		if(resultado.length > 0){
		    res.write("Ya existe el usuario");
		    res.end();
                }else{
		    db.connect(function(err) {
           	        var sql = "INSERT INTO usuario(nombre, contra) VALUES (" + mysql.escape(nombre) + ", " + mysql.escape(contra) + ");";
          	        db.query(sql, function (error) {
                	    res.write("Correcto");
			    res.end();
            	        });
        	    });
		}
            });
        });
    }else{
        res.write("Datos incorrectos");
        res.end();
    };
});


app.post('/guardarNivel', (req, res) => {
    var usuario = req.body.usuario;
    var nivel = req.body.nivel;
    var estado = req.body.estado;

    if (req.body.usuario != null && usuario != "" && nivel != ""){
        var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });


        db.connect(function(err) {
	    if(estado === "1"){
                var sql = "INSERT INTO favoritos VALUES (" + mysql.escape(usuario) + ", " + mysql.escape(nivel) + ");";
	    } else {
		var sql = "DELETE FROM favoritos WHERE usuario = " + mysql.escape(usuario) + "AND id_nivel = " + mysql.escape(nivel);
	    }
            db.query(sql, function (error,resultado) {
		console.log(error);
                res.write("Correcto");
                res.end();
            });
        });
    }else{
        res.write("Datos incorrectos");
        res.end();
    };
});


app.post('/gustarNivel', (req, res) => {
    var usuario = req.body.usuario;
    var nivel = req.body.nivel;
    var estado = req.body.estado;
    if (req.body.usuario != null && usuario != "" && nivel != ""){
        var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });

	console.log(estado);
        db.connect(function(err) {
            if(estado === "1"){
                var sql = "INSERT INTO megusta VALUES (" + mysql.escape(usuario) + ", " + mysql.escape(nivel) + ");";
            } else {
                var sql = "DELETE FROM megusta WHERE usuario = " + mysql.escape(usuario) + "AND id_nivel = " + mysql.escape(nivel);
            }
            db.query(sql, function (error,resultado) {
		console.log(error);
                res.write("Correcto");
                res.end();
            });
        });
    }else{
        res.write("Datos incorrectos");
        res.end();
    };
});


app.post('/borrarNivel', (req, res) => {
    var nivel = req.body.nivel;

    if (req.body.nivel != null && nivel != ""){
        var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });


        db.connect(function(err) {
            var sql = "DELETE FROM favoritos WHERE id_nivel = " + mysql.escape(nivel);
            db.query(sql, function (error,resultado) {
		console.log(error);
		var sql = "DELETE FROM megusta WHERE id_nivel = " + mysql.escape(nivel);
                db.query(sql, function (error,resultado) {
		    console.log(error);
		    var sql = "DELETE FROM nivel WHERE id = " + mysql.escape(nivel);
                    db.query(sql, function (error,resultado) {
			console.log(error);
			res.write("Correcto");
			res.end();
                    });
                });
            });
        });
    }else{
        res.write("Datos incorrectos");
        res.end();
    };
});


app.post('/inicioSesion', (req, res) => {
    var nombre = req.body.nombre;
    var contra = req.body.contra;

    if (req.body.nombre != null && nombre != "" && contra != ""){
        var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });


        db.connect(function(err) {
            var sql = "SELECT nombre, contra FROM usuario WHERE nombre=" + mysql.escape(nombre);
            db.query(sql, function (error,resultado) {
                if(resultado.length == 0){
                    res.write("No existe el usuario");
                    res.end();
                }else if (resultado[0].contra !== contra){
		    res.write("Contraseña incorrecta");
		    res.end();
		}else{
		    res.write("Correcto");
		    res.end();
		};
            });
        });
    }else{
        res.write("Datos incorrectos");
        res.end();
    }
});


app.post('/addNivel', (req, res) => {
    var nombre = req.body.nombre;
    var string = req.body.string;
    var creador = req.body.creador;

    if (req.body.nombre != null && nombre != "" && string != ""){
        var db = mysql.createConnection({
            host: "localhost",
            user: "XXXX",
            database: "XXXX",
            password: "XXXX"
        });

	db.connect(function(err) {
            var sql = "INSERT INTO nivel(id, string, nombre, creador) VALUES (0, " + mysql.escape(string) + ", " + mysql.escape(nombre) + ", " + mysql.escape(creador) + ");";
            db.query(sql, function (error) {
		console.log(error);
                res.write("Correcto");
                res.end();
            });
        });
    }else{
        res.write("Datos incorrectos");
        res.end();
    };
});
