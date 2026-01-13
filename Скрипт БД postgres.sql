-- DROP SCHEMA public;

CREATE SCHEMA public AUTHORIZATION pg_database_owner;

COMMENT ON SCHEMA public IS 'standard public schema';

-- DROP SEQUENCE "AgentPriorityHistory_ID_seq";

CREATE SEQUENCE "AgentPriorityHistory_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "AgentType_ID_seq";

CREATE SEQUENCE "AgentType_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "Agent_ID_seq";

CREATE SEQUENCE "Agent_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "MaterialCountHistory_ID_seq";

CREATE SEQUENCE "MaterialCountHistory_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "MaterialType_ID_seq";

CREATE SEQUENCE "MaterialType_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "Material_ID_seq";

CREATE SEQUENCE "Material_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "ProductCostHistory_ID_seq";

CREATE SEQUENCE "ProductCostHistory_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "ProductSale_ID_seq";

CREATE SEQUENCE "ProductSale_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "ProductType_ID_seq";

CREATE SEQUENCE "ProductType_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "Product_ID_seq";

CREATE SEQUENCE "Product_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "Shop_ID_seq";

CREATE SEQUENCE "Shop_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE "Supplier_ID_seq";

CREATE SEQUENCE "Supplier_ID_seq"
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE clients_client_id_seq;

CREATE SEQUENCE clients_client_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE diagnosis_diagnosis_id_seq;

CREATE SEQUENCE diagnosis_diagnosis_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE medical_departments_medical_department_id_seq;

CREATE SEQUENCE medical_departments_medical_department_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE users_type_user_type_id_seq;

CREATE SEQUENCE users_type_user_type_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	START 1
	CACHE 1
	NO CYCLE;
-- DROP SEQUENCE users_user_id_seq;

CREATE SEQUENCE users_user_id_seq
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 9223372036854775807
	START 1
	CACHE 1
	NO CYCLE;-- public."AgentType" определение

-- Drop table

-- DROP TABLE "AgentType";

CREATE TABLE "AgentType" ( "ID" serial4 NOT NULL, "Title" varchar(50) NOT NULL, "Image" varchar(100) NULL, CONSTRAINT "AgentType_pkey" PRIMARY KEY ("ID"));


-- public."MaterialType" определение

-- Drop table

-- DROP TABLE "MaterialType";

CREATE TABLE "MaterialType" ( "ID" serial4 NOT NULL, "Title" varchar(50) NOT NULL, "DefectedPercent" float8 NOT NULL, CONSTRAINT "MaterialType_pkey" PRIMARY KEY ("ID"));


-- public."ProductType" определение

-- Drop table

-- DROP TABLE "ProductType";

CREATE TABLE "ProductType" ( "ID" serial4 NOT NULL, "Title" varchar(50) NOT NULL, "DefectedPercent" float8 NOT NULL, CONSTRAINT "ProductType_pkey" PRIMARY KEY ("ID"));


-- public."Supplier" определение

-- Drop table

-- DROP TABLE "Supplier";

CREATE TABLE "Supplier" ( "ID" serial4 NOT NULL, "Title" varchar(150) NOT NULL, "INN" varchar(12) NOT NULL, "StartDate" date NOT NULL, "QualityRating" int4 NULL, "SupplierType" varchar(20) NULL, CONSTRAINT "Supplier_pkey" PRIMARY KEY ("ID"));


-- public."Agent" определение

-- Drop table

-- DROP TABLE "Agent";

CREATE TABLE "Agent" ( "ID" serial4 NOT NULL, "Title" varchar(150) NOT NULL, "AgentTypeID" int4 NOT NULL, "Address" varchar(300) NULL, "INN" varchar(12) NOT NULL, "KPP" varchar(9) NULL, "DirectorName" varchar(100) NULL, "Phone" varchar(20) NOT NULL, "Email" varchar(255) NULL, "Logo" varchar(100) NULL, "Priority" int4 NOT NULL, CONSTRAINT "Agent_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_Agent_AgentType" FOREIGN KEY ("AgentTypeID") REFERENCES "AgentType"("ID"));


-- public."AgentPriorityHistory" определение

-- Drop table

-- DROP TABLE "AgentPriorityHistory";

CREATE TABLE "AgentPriorityHistory" ( "ID" serial4 NOT NULL, "AgentID" int4 NOT NULL, "ChangeDate" timestamp(6) NOT NULL, "PriorityValue" int4 NOT NULL, CONSTRAINT "AgentPriorityHistory_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_AgentPriorityHistory_Agent" FOREIGN KEY ("AgentID") REFERENCES "Agent"("ID"));


-- public."Material" определение

-- Drop table

-- DROP TABLE "Material";

CREATE TABLE "Material" ( "ID" serial4 NOT NULL, "Title" varchar(100) NOT NULL, "CountInPack" int4 NOT NULL, "Unit" varchar(10) NOT NULL, "CountInStock" float8 NULL, "MinCount" float8 NOT NULL, "Description" text NULL, "Cost" numeric(10, 2) NOT NULL, "Image" varchar(100) NULL, "MaterialTypeID" int4 NOT NULL, CONSTRAINT "Material_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_Material_MaterialType" FOREIGN KEY ("MaterialTypeID") REFERENCES "MaterialType"("ID"));


-- public."MaterialCountHistory" определение

-- Drop table

-- DROP TABLE "MaterialCountHistory";

CREATE TABLE "MaterialCountHistory" ( "ID" serial4 NOT NULL, "MaterialID" int4 NOT NULL, "ChangeDate" timestamp(6) NOT NULL, "CountValue" float8 NOT NULL, CONSTRAINT "MaterialCountHistory_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_MaterialCountHistory_Material" FOREIGN KEY ("MaterialID") REFERENCES "Material"("ID"));


-- public."MaterialSupplier" определение

-- Drop table

-- DROP TABLE "MaterialSupplier";

CREATE TABLE "MaterialSupplier" ( "MaterialID" int4 NOT NULL, "SupplierID" int4 NOT NULL, CONSTRAINT "MaterialSupplier_pkey" PRIMARY KEY ("MaterialID", "SupplierID"), CONSTRAINT "FK_MaterialSupplier_Material" FOREIGN KEY ("MaterialID") REFERENCES "Material"("ID"), CONSTRAINT "FK_MaterialSupplier_Supplier" FOREIGN KEY ("SupplierID") REFERENCES "Supplier"("ID"));


-- public."Product" определение

-- Drop table

-- DROP TABLE "Product";

CREATE TABLE "Product" ( "ID" serial4 NOT NULL, "Title" varchar(100) NOT NULL, "ProductTypeID" int4 NULL, "ArticleNumber" varchar(10) NOT NULL, "Description" text NULL, "Image" varchar(100) NULL, "ProductionPersonCount" int4 NULL, "ProductionWorkshopNumber" int4 NULL, "MinCostForAgent" numeric(10, 2) NOT NULL, CONSTRAINT "Product_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_Product_ProductType" FOREIGN KEY ("ProductTypeID") REFERENCES "ProductType"("ID"));


-- public."ProductCostHistory" определение

-- Drop table

-- DROP TABLE "ProductCostHistory";

CREATE TABLE "ProductCostHistory" ( "ID" serial4 NOT NULL, "ProductID" int4 NOT NULL, "ChangeDate" timestamp(6) NOT NULL, "CostValue" numeric(10, 2) NOT NULL, CONSTRAINT "ProductCostHistory_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_ProductCostHistory_Product" FOREIGN KEY ("ProductID") REFERENCES "Product"("ID"));


-- public."ProductMaterial" определение

-- Drop table

-- DROP TABLE "ProductMaterial";

CREATE TABLE "ProductMaterial" ( "ProductID" int4 NOT NULL, "MaterialID" int4 NOT NULL, "Count" float8 NULL, CONSTRAINT "ProductMaterial_pkey" PRIMARY KEY ("ProductID", "MaterialID"), CONSTRAINT "FK_ProductMaterial_Material" FOREIGN KEY ("MaterialID") REFERENCES "Material"("ID"), CONSTRAINT "FK_ProductMaterial_Product" FOREIGN KEY ("ProductID") REFERENCES "Product"("ID"));


-- public."ProductSale" определение

-- Drop table

-- DROP TABLE "ProductSale";

CREATE TABLE "ProductSale" ( "ID" serial4 NOT NULL, "AgentID" int4 NOT NULL, "ProductID" int4 NOT NULL, "SaleDate" date NOT NULL, "ProductCount" int4 NOT NULL, CONSTRAINT "ProductSale_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_ProductSale_Agent" FOREIGN KEY ("AgentID") REFERENCES "Agent"("ID"), CONSTRAINT "FK_ProductSale_Product" FOREIGN KEY ("ProductID") REFERENCES "Product"("ID"));


-- public."Shop" определение

-- Drop table

-- DROP TABLE "Shop";

CREATE TABLE "Shop" ( "ID" serial4 NOT NULL, "Title" varchar(150) NOT NULL, "Address" varchar(300) NULL, "AgentID" int4 NOT NULL, CONSTRAINT "Shop_pkey" PRIMARY KEY ("ID"), CONSTRAINT "FK_Shop_Agent" FOREIGN KEY ("AgentID") REFERENCES "Agent"("ID"));