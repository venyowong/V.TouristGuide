-- Table: public.operation

DROP TABLE IF EXISTS public.operation;
CREATE SEQUENCE IF NOT EXISTS operation_id_seq;

CREATE TABLE IF NOT EXISTS public.operation
(
    id bigint NOT NULL DEFAULT nextval('operation_id_seq'::regclass),
    user_id bigint NOT NULL,
    place_id bigint NOT NULL,
    type integer NOT NULL DEFAULT 0,
    remark character varying(1000) COLLATE pg_catalog."default",
    is_valid boolean NOT NULL DEFAULT true,
    create_time timestamp with time zone NOT NULL DEFAULT now(),
    update_time timestamp with time zone NOT NULL DEFAULT now(),
    CONSTRAINT operation_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.operation
    OWNER to postgres;
-- Index: operation_place_idx

DROP INDEX IF EXISTS public.operation_place_idx;

CREATE INDEX IF NOT EXISTS operation_place_idx
    ON public.operation USING btree
    (place_id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: operation_user_idx

DROP INDEX IF EXISTS public.operation_user_idx;

CREATE INDEX IF NOT EXISTS operation_user_idx
    ON public.operation USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;

-- Table: public.place

DROP TABLE IF EXISTS public.place;
CREATE SEQUENCE IF NOT EXISTS place_id_seq;

CREATE TABLE IF NOT EXISTS public.place
(
    id bigint NOT NULL DEFAULT nextval('place_id_seq'::regclass),
    name character varying(200) COLLATE pg_catalog."default" NOT NULL,
    cover character varying(500) COLLATE pg_catalog."default",
    open_time character varying(200) COLLATE pg_catalog."default",
    address character varying(500) COLLATE pg_catalog."default" NOT NULL,
    lng numeric(17,14) NOT NULL,
    lat numeric(17,14) NOT NULL,
    tel character varying(50) COLLATE pg_catalog."default",
    type integer NOT NULL,
    is_valid boolean NOT NULL DEFAULT true,
    create_time timestamp with time zone NOT NULL DEFAULT now(),
    update_time timestamp with time zone NOT NULL DEFAULT now(),
    visibility numeric(10,2) NOT NULL DEFAULT 10000,
    region character varying(20) COLLATE pg_catalog."default",
    CONSTRAINT place_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.place
    OWNER to postgres;
-- Index: place_id_idx

DROP INDEX IF EXISTS public.place_id_idx;

CREATE UNIQUE INDEX IF NOT EXISTS place_id_idx
    ON public.place USING btree
    (id ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: place_is_valid_idx

DROP INDEX IF EXISTS public.place_is_valid_idx;

CREATE INDEX IF NOT EXISTS place_is_valid_idx
    ON public.place USING btree
    (is_valid ASC NULLS LAST)
    TABLESPACE pg_default;

-- Table: public.place_addition

DROP TABLE IF EXISTS public.place_addition;
CREATE SEQUENCE IF NOT EXISTS place_addition_id_seq;

CREATE TABLE IF NOT EXISTS public.place_addition
(
    id bigint NOT NULL DEFAULT nextval('place_addition_id_seq'::regclass),
    place_id bigint NOT NULL,
    type character varying(20) COLLATE pg_catalog."default" NOT NULL,
    title character varying(50) COLLATE pg_catalog."default",
    img character varying(500) COLLATE pg_catalog."default",
    url character varying(500) COLLATE pg_catalog."default",
    "desc" character varying(1000) COLLATE pg_catalog."default",
    source character varying(50) COLLATE pg_catalog."default",
    is_valid boolean NOT NULL DEFAULT true,
    create_time timestamp with time zone NOT NULL DEFAULT now(),
    update_time timestamp with time zone NOT NULL DEFAULT now(),
    source_url character varying(500) COLLATE pg_catalog."default",
    CONSTRAINT place_addition_pkey PRIMARY KEY (id)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.place_addition
    OWNER to postgres;
-- Index: place_addition_id_type_idx

DROP INDEX IF EXISTS public.place_addition_id_type_idx;

CREATE INDEX IF NOT EXISTS place_addition_id_type_idx
    ON public.place_addition USING btree
    (place_id ASC NULLS LAST, type COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: place_addition_is_valid_idx

DROP INDEX IF EXISTS public.place_addition_is_valid_idx;

CREATE INDEX IF NOT EXISTS place_addition_is_valid_idx
    ON public.place_addition USING btree
    (is_valid ASC NULLS LAST)
    TABLESPACE pg_default;