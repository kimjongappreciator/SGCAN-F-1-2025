
\c f1_database

CREATE TABLE IF NOT EXISTS public."Files"
(
    "FileName" character varying COLLATE pg_catalog."default" NOT NULL,
    "UploadDate" date NOT NULL,
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "Files_pkey" PRIMARY KEY ("Id")
);


CREATE TABLE IF NOT EXISTS public."User"
(
    "Name" character varying COLLATE pg_catalog."default" NOT NULL,
    "Email" character varying COLLATE pg_catalog."default" NOT NULL,
    "Password" character varying COLLATE pg_catalog."default" NOT NULL,
    "Id" uuid NOT NULL,
    CONSTRAINT "User_pkey" PRIMARY KEY ("Id")
);