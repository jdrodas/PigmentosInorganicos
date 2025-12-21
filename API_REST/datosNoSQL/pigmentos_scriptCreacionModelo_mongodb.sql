-- Scripts de clase - Diciembre 18 de 2025 
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Pigmentos Inorgánicos
-- Motor de Base de datos: MongoDB 8.x

-- ***********************************
-- Creación del modelo de datos
-- ***********************************


-- Con el usuario pigmentos_app

-- ****************************************
-- Creación de Colecciones
-- ****************************************

- Colección: colores
db.createCollection("colores",{
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                title: 'Registro de los colores',
                required: [                    
                    "_id",
                    "nombre",
                    "representacion_hexadecimal"
                ],
                properties: {
                    _id: {
                        bsonType: 'objectId'
                    },
                    nombre: {
                        bsonType: 'string',
                        description: "'nombre' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 2
                    },
                    representacion_hexadecimal: {
                        bsonType: 'string',
                        description: "'representacion_hexadecimal' debe ser una cadena de caracteres y no puede ser nulo",
                        pattern: "^#([A-Fa-f0-9]{6})$",
                        minLength: 7,
                        maxLength: 7
                    }
                },
                additionalProperties: false
            }
        }
    } 
);

-- Coleccion: familias_quimicas
db.createCollection("familias_quimicas", {
  validator: {
    $jsonSchema: {
      bsonType: "object",
      title: "Familias químicas",
      required: ["nombre","descripcion"],
      properties: {
        _id: { bsonType: "objectId" },
        nombre: { bsonType: "string", minLength: 2 },
        descripcion: { bsonType: "string", minLength: 1 }
      },
      additionalProperties: false
    }
  }
});

-- Coleccion: familias_quimicas
db.createCollection("familias_quimicas",{
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                title: 'Registro de las familias químicas',
                required: [                    
                    "_id",
                    "nombre",
                    "composicion"
                ],
                properties: {
                    _id: {
                        bsonType: 'objectId'
                    },
                    nombre: {
                        bsonType: 'string',
                        description: "'nombre' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 2
                    },
                    composicion: {
                        bsonType: 'string',
                        description: "'composicion' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 2
                    }
                },
                additionalProperties: false
            }
        }
    } 
);


-- Coleccion: pigmentos
db.createCollection("pigmentos",{
        validator: {
            $jsonSchema: {
                bsonType: 'object',
                title: 'Registro de los pigmentos inorgánicos',
                required: [                    
                    "_id",
                    "nombre",
                    "formula_quimica",
                    "numero_ci",
                    "familia_quimica_id",
                    "color_id"
                ],
                properties: {
                    _id: {
                        bsonType: 'objectId'
                    },
                    nombre: {
                        bsonType: 'string',
                        description: "'nombre' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 2
                    },
                    formula_quimica: {
                        bsonType: 'string',
                        description: "'formula_quimica' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 2
                    },
                    numero_ci: {
                        bsonType: 'string',
                        description: "'numero_ci' debe ser una cadena de caracteres y no puede ser nulo",
                        minLength: 2
                    },                    
                    familia_quimica_id: {
                        bsonType: ["objectId","string"],
                        description: "Id de la familia química del pigmento",
                        minLength: 3
                    },                    
                    color_id: {
                        bsonType: ["objectId","string"],
                        description: "Id del color del pigmento"
                    }
                },
                additionalProperties: false
            }
        }
    } 
);


-- ********************************************************
-- Sentencias de migración desde modelo relacional
-- ********************************************************

-- Crear colección básica e importar json inicial para luego 
-- relacionar los ObjectId de colores y familias

-- Actualizamos el objectId del color para los pigmentos
db.pigmentos.find().forEach(function(pigmento){
  let color = db.colores.findOne({"nombre":pigmento.color_nombre});

  if(color){
    db.pigmentos.updateOne(
      {_id:pigmento._id},
      {$set:{"color_id": color._id}}
    )
  }
}
);

-- Actualizamos el objectId de la familia química para los pigmentos
db.pigmentos.find().forEach(function(pigmento){
  let familia = db.familias_quimicas.findOne({"nombre":pigmento.familia_quimica_nombre});

  if(familia){
    db.pigmentos.updateOne(
      {_id:pigmento._id},
      {$set:{"familia_quimica_id": familia._id}}
    )
  }
}
);

-- Vinculamos información del color directamente en el documento de pigmentos
db.pigmentos.find().forEach(function(pigmento){
    let color = db.colores.findOne({_id:pigmento.color_id});

    if (color){
        db.pigmentos.updateOne(
            {_id:pigmento._id},
            {$set:{
                "color_nombre": color.nombre,
                "color_representacion_hexadecimal": color.representacion_hexadecimal
                }
            }
        )
    }
}
);

-- Vinculamos información de la familia directamente en el documento de pigmentos
db.pigmentos.find().forEach(function(pigmento){
    let familia = db.familias_quimicas.findOne({_id:pigmento.familia_quimica_id});

    if (familia){
        db.pigmentos.updateOne(
            {_id:pigmento._id},
            {$set:{
                "familia_quimica_nombre": familia.nombre,
                "familia_quimica_composicion": familia.composicion
                }
            }
        )
    }
}
);

-- Retirar campos no requeridos, en caso de que existan
db.pigmentos.updateMany({}, { $unset: { color_id: "" } });
db.pigmentos.updateMany({}, { $unset: { familia_quimica_id: "" } });
db.pigmentos.updateMany({}, { $unset: { familia_quimica_nombre: "" } });
db.pigmentos.updateMany({}, { $unset: { familia_quimica_composicion: "" } });
db.pigmentos.updateMany({}, { $unset: { color_nombre: "" } });
db.pigmentos.updateMany({}, { $unset: { color_representacion_hexadecimal: "" } });