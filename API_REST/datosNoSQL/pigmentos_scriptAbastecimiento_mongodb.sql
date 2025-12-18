-- Scripts de clase - Diciembre 18 de 2025 
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Pigmentos Inorgánicos
-- Motor de Base de datos: MongoDB 8.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************

-- Motor de Base de datos: MongoDB 8.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull mongodb/mongodb-community-server

-- Crear el contenedor
docker run --name mongodb_pigmentos -e “MONGO_INITDB_ROOT_USERNAME=mongoadmin” -e MONGO_INITDB_ROOT_PASSWORD=unaClav3 -p 27017:27017 -d mongodb/mongodb-community-server:latest

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Para conectarse al contenedor
mongodb://mongoadmin:unaClav3@localhost:27017/

-- Con usuario mongoadmin:

-- Para saber que versión de Mongo se está usando - 8.0.13 a dic/25
db.version()

-- crear la base de datos
use pigmentos_db;

-- Crear el rol para el usuario de gestion de Documentos en las colecciones
db.createRole(
  {
    role: "GestorDocumentos",
    privileges: [
        {
            resource: { 
                db: "pigmentos_db", 
                collection: "" 
            }, 
            actions: [
                "find", 
                "insert", 
                "update", 
                "remove",
                "listCollections"
            ]
        }
    ],
    roles: []
  }
);

-- Crear usuario para gestionar el modelo

db.createUser({
  user: "pigmentos_app",
  pwd: "unaClav3",  
  roles: [
    { role: "readWrite", db: "pigmentos_db" },
    { role: "dbAdmin", db: "pigmentos_db" }
  ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

db.createUser(
  {
    user: "pigmentos_usr",
    pwd: "unaClav3",
    roles: [ 
    { role: "GestorDocumentos", db: "pigmentos_db" }
    ],
    mechanisms: ["SCRAM-SHA-256"]
  }
);

-- Para saber que usuarios hay creados en la base de datos
db.getUsers()