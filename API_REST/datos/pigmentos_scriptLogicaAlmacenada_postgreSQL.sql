
-- Scripts de clase - Diciembre 4 de 2025
-- Curso de Tópicos Avanzados de base de datos - UPB 202520
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Pigmentos Inorgánicos
-- Motor de Base de datos: PostgreSQL 17.x

-- ***********************************
-- Creación de procedimientos
-- *********************************** 

-- Con el usuario pigmentos_app

-- ### Colores ####

-- p_inserta_color
create or replace procedure core.p_inserta_color(
                            in p_nombre                 text,
                            in p_representacion_hex     text)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_representacion_hex is null or
           length(p_nombre) = 0 or
           length(p_representacion_hex) = 0 then
               raise exception 'El nombre del color o su representación hexadecimal no pueden ser nulos';
        end if;

        select count(id) into l_total_registros
        from core.colores
        where lower(p_nombre) = lower(nombre);

        if l_total_registros != 0  then
            raise exception 'ya existe ese color registrado con ese nombre';
        end if;

        select count(id) into l_total_registros
        from core.colores
        where lower(p_representacion_hex) = lower(representacion_hexadecimal);

        if l_total_registros != 0  then
            raise exception 'ya existe ese color registrado con esa representación hexadecimal';
        end if;

        insert into core.colores (nombre,representacion_hexadecimal)
        values (initcap(p_nombre), upper(p_representacion_hex));
    end;
$$;

-- p_actualiza_color
create or replace procedure core.p_actualiza_color(
                            in p_id                     uuid,
                            in p_nombre                 text, 
                            in p_representacion_hex     text)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_representacion_hex is null or
           length(p_nombre) = 0 or
           length(p_representacion_hex) = 0 then
               raise exception 'El nombre del color o su representación hexadecimal no pueden ser nulos';
        end if;

        select count(id) into l_total_registros
        from core.colores
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un color con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.colores
        where lower(nombre) = lower(p_nombre)
        and upper(representacion_hexadecimal) = upper(p_representacion_hex)
        and id != p_id;

        if l_total_registros > 0  then
            raise exception 'Ya existe un color registrado con ese nombre y representación hexadecimal';
        end if;

        update core.colores
        set
            nombre = initcap(p_nombre),
            representacion_hexadecimal = upper(p_representacion_hex)
        where id = p_id;
    end;
$$;

-- p_elimina_color
create or replace procedure core.p_elimina_color(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        select count(id) into l_total_registros
        from core.colores
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un color con ese Id';
        end if;

        select count(color_id) into l_total_registros
        from core.v_info_pigmentos
        where color_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay pigmentos que dependen de este color.';
        end if;

        delete from core.colores
        where id = p_id;
    end;
$$;

-- ### familias_quimicas ####

-- p_inserta_familia_quimica
create or replace procedure core.p_inserta_familia_quimica(
                            in p_nombre                 text, 
                            in p_composicion            text)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_composicion is null or
           length(p_nombre) = 0 or
           length(p_composicion) = 0 then
               raise exception 'El nombre de la familia química o su composición no pueden ser nulos';
        end if;

        select count(id) into l_total_registros
        from core.familias_quimicas
        where lower(p_nombre) = lower(nombre);

        if l_total_registros != 0  then
            raise exception 'ya existe ese familia química registrada';
        end if;

        insert into core.familias_quimicas (nombre,composicion)
        values (initcap(p_nombre), initcap(p_composicion));
    end;
$$;

-- p_actualiza_familia_quimica
create or replace procedure core.p_actualiza_familia_quimica(
                            in p_id                     uuid,
                            in p_nombre                 text, 
                            in p_composicion            text)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_composicion is null or
           length(p_nombre) = 0 or
           length(p_composicion) = 0 then
               raise exception 'El nombre de la familia química o su composición no pueden ser nulos';
        end if;

        select count(id) into l_total_registros
        from core.familias_quimicas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una familia química con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.familias_quimicas
        where lower(nombre) = lower(p_nombre)
        and lower(composicion) = lower(p_composicion)
        and id != p_id;

        if l_total_registros > 0  then
            raise exception 'Ya existe una familia química con ese nombre y composición';
        end if;

        update core.familias_quimicas
        set
            nombre = initcap(p_nombre),
            composicion = initcap(composicion)
        where id = p_id;
    end;
$$;

-- -- p_elimina_familia_quimica
create or replace procedure core.p_elimina_familia_quimica(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        select count(id) into l_total_registros
        from core.familias_quimicas
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe una familia química con ese Id';
        end if;

        select count(color_id) into l_total_registros
        from core.v_info_pigmentos
        where familia_quimica_id = p_id;

        if l_total_registros != 0  then
            raise exception 'No se puede eliminar, hay pigmentos que dependen de esta familia química.';
        end if;

        delete from core.familias_quimicas
        where id = p_id;
    end;
$$;

-- ### pigmentos ####

-- p_inserta_pigmento
create or replace procedure core.p_inserta_pigmento(
                            in p_nombre                 text, 
                            in p_formula_quimica        text,
                            in p_numero_ci              text,
                            in p_familia_quimica_id     uuid,                             
                            in p_color_id               uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        if p_nombre is null or
           p_formula_quimica is null or
           p_numero_ci is null or
           length(p_nombre) = 0 or
           length(p_formula_quimica) = 0 or
           length(p_numero_ci) = 0 then
               raise exception 'El nombre del pigmento, su fórmula química o su número CI no pueden ser nulos';
        end if;

        select count(id) into l_total_registros
        from core.pigmentos
        where lower(p_nombre) = lower(nombre)
        and p_familia_quimica_id = familia_quimica_id
        and p_color_id = color_id;

        if l_total_registros != 0  then
            raise exception 'ya existe un pigmento con ese nombre, color y familia química';
        end if;

        select count(id) into l_total_registros
        from core.colores
        where p_color_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe un color con ese ID';
        end if;        

        select count(id) into l_total_registros
        from core.familias_quimicas
        where p_familia_quimica_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe una familia química con ese ID';
        end if;        

        insert into core.pigmentos (nombre, formula_quimica, numero_ci, familia_quimica_id, color_id)
        values (initcap(p_nombre), p_formula_quimica, upper(p_numero_ci), p_familia_quimica_id, p_color_id);
    end;
$$;


-- -- p_actualiza_pigmento
create procedure core.p_actualiza_pigmento(
                            in p_id                     uuid,    
                            in p_nombre                 text, 
                            in p_formula_quimica        text,
                            in p_numero_ci              text,
                            in p_familia_quimica_id     uuid,                             
                            in p_color_id               uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        select count(id) into l_total_registros
        from core.pigmentos
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un pigmento registrado con ese Id';
        end if;

        select count(id) into l_total_registros
        from core.familias_quimicas
        where p_familia_quimica_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe una familia química con ese Id';
        end if;        

        -- Validamos que la ubicación se válida
        select count(id) into l_total_registros
        from core.colores
        where p_color_id = id;        

        if l_total_registros = 0  then
            raise exception 'no existe un color con ese Id';
        end if;          

        if p_nombre is null or
           p_formula_quimica is null or
           p_numero_ci is null or
           length(p_nombre) = 0 or
           length(p_formula_quimica) = 0 or
           length(p_numero_ci) = 0 then
               raise exception 'El nombre del pigmento, su fórmula química o su número CI no pueden ser nulos';
        end if;      

        select count(id) into l_total_registros
        from core.pigmentos
        where lower(p_nombre) = lower(nombre)
        and p_familia_quimica_id = familia_quimica_id
        and p_color_id = color_id;

        if l_total_registros != 0  then
            raise exception 'ya existe un pigmento con ese nombre, color y familia química';
        end if;

        update core.pigmentos
        set
            nombre = initcap(p_nombre),
            formula_quimica = p_formula_quimica,
            numero_ci = upper(p_numero_ci),
            familia_quimica_id = p_familia_quimica_id,
            color_id = p_color_id
        where p_id = id;     
    end;
$$;

-- -- p_elimina_pigmento
create procedure core.p_elimina_pigmento(
                            in p_id                     uuid)
language plpgsql as
$$
    declare
        l_total_registros integer;

    begin
        select count(id) into l_total_registros
        from core.pigmentos
        where id = p_id;

        if l_total_registros = 0  then
            raise exception 'No existe un pigmento con ese Guid';
        end if;

        delete from core.pigmentos
        where id = p_id;
    end;
$$;