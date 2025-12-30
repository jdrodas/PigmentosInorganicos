
-- Scripts de clase - Noviembre 27 de 2025
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Pigmentos Inorgánicos
-- Motor de Base de datos: PostgreSQL 17.x

-- ***********************************
-- Creación del modelo de datos
-- ***********************************


-- Con el usuario pigmentos_app

-- ****************************************
-- Creación de Tablas
-- ****************************************


create table core.colores
(
    id                              uuid default gen_random_uuid() constraint colores_pk primary key,
    nombre                          text not null,
    representacion_hexadecimal      text not null
);

comment on table "colores" is 'Registro de los colores';
comment on column "colores"."id" is 'Identificador único del color';
comment on column "colores"."nombre" is 'Nombre del color';
comment on column "colores"."representacion_hexadecimal" is 'Representación del color para visualización web';


create table core.familias_quimicas
(
    id                              uuid default gen_random_uuid() constraint familias_quimicas_pk primary key,
    nombre                          text not null,
    composicion                     text not null
);

comment on table "familias_quimicas" is 'Registro de las familias químicas';
comment on column "familias_quimicas"."id" is 'identificador único de la familia química';
comment on column "familias_quimicas"."nombre" is 'nombre de la familia química';
comment on column "familias_quimicas"."composicion" is 'detalle de la composición de la familia química';


create table core.pigmentos
(
    id                              uuid default gen_random_uuid() not null constraint pigmentos_pk primary key,
    nombre                          text not null,
    formula_quimica                 text not null,
    numero_ci                       text not null,
    familia_quimica_id              uuid not null constraint pigmentos_familias_fk references familias_quimicas,
    color_id                        uuid not null constraint pigmentos_colores_fk references colores
);
    
comment on table "pigmentos" is 'registro de los pigmentos inorgánicos';
comment on column "pigmentos"."id" is 'id del pigmento';
comment on column "pigmentos"."nombre" is 'nombre comercial del pigmento';
comment on column "pigmentos"."formula_quimica" is 'fórmula química del pigmento';
comment on column "pigmentos"."numero_ci" is 'código ci del pigmento.';
comment on column "pigmentos"."familia_quimica_id" is 'identificador único de la familia química';
comment on column "pigmentos"."color_id" is 'identificador único del color principal';

-- ****************************************
-- Creación de Vistas
-- ****************************************

create view core.v_info_pigmentos as
(
    select distinct
        p.id pigmento_id,
        p.nombre pigmento_nombre,
        p.formula_quimica pigmento_formula_quimica,
        p.numero_ci pigmento_numero_ci, 
        p.familia_quimica_id,
        fq.nombre familia_quimica_nombre,
        fq.composicion familia_quimica_composicion,
        p.color_id,
        c.nombre color_nombre,
        c.representacion_hexadecimal color_representacion_hexadecimal
from core.pigmentos p
    join core.familias_quimicas fq on p.familia_quimica_id = fq.id
    join core.colores c on p.color_id = c.id
);