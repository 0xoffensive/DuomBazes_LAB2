#@(#) script.ddl

DROP TABLE IF EXISTS MOKEJIMAS;
DROP TABLE IF EXISTS DETALES_KIEKIS;
DROP TABLE IF EXISTS turi;
DROP TABLE IF EXISTS SASKAITA;
DROP TABLE IF EXISTS ATLIKTAS_DARBAS;
DROP TABLE IF EXISTS aptarnauja;
DROP TABLE IF EXISTS REMONTO_SUTARTIS;
DROP TABLE IF EXISTS AUTOMOBILIS;
DROP TABLE IF EXISTS MODELIS;
DROP TABLE IF EXISTS MECHANIKAS;
DROP TABLE IF EXISTS GEDIMAS;
DROP TABLE IF EXISTS DETALE;
DROP TABLE IF EXISTS DARBUOTOJAS;
DROP TABLE IF EXISTS saskaitos_busena;
DROP TABLE IF EXISTS remonto_busenos;
DROP TABLE IF EXISTS pavaru_dezes;
DROP TABLE IF EXISTS kuro_tipai;
DROP TABLE IF EXISTS kebulu_tipai;
DROP TABLE IF EXISTS gedimo_tipai;
DROP TABLE IF EXISTS detaliu_bukles;
DROP TABLE IF EXISTS SERVISAS;
DROP TABLE IF EXISTS KLIENTAS;
DROP TABLE IF EXISTS GAMINTOJAS;
CREATE TABLE GAMINTOJAS
(
	pavadinimas varchar (255) NOT NULL,
	ID_ integer AUTO_INCREMENT,
	PRIMARY KEY(ID_)
);

CREATE TABLE KLIENTAS
(
	asmens_kodas varchar(255) NOT NULL,
	vardas varchar (255) NOT NULL,
	pavarde varchar (255) NOT NULL,
	telefonas varchar (255) NOT NULL,
	epastas varchar (255),
	PRIMARY KEY(asmens_kodas)
);

CREATE TABLE SERVISAS
(
	pavadinimas varchar (255) NOT NULL,
	telefonas varchar (255) NOT NULL,
	epastas varchar (255) NOT NULL,
	miestas varchar (255) NOT NULL,
	gatve varchar (255) NOT NULL,
	pastato_nr varchar (255) NOT NULL,
	pasto_kodas varchar (255) NOT NULL,
	ID_ integer AUTO_INCREMENT,
	PRIMARY KEY(ID_)
);

CREATE TABLE DARBUOTOJAS
(
	vardas varchar (255) NOT NULL,
	pavarde varchar (255) NOT NULL,
	telefonas varchar (255) NOT NULL,
	tabelio_nr integer,
	fk_SERVISASID_ integer NOT NULL,
	PRIMARY KEY(tabelio_nr),
	CONSTRAINT fkc_serviso_darbuotojas FOREIGN KEY(fk_SERVISASID_) REFERENCES SERVISAS (ID_)
);
ALTER TABLE DARBUOTOJAS AUTO_INCREMENT=100;

CREATE TABLE DETALE
(
	pavadinimas varchar (255),
	detales_kaina decimal (10, 2),
	detales_bukle ENUM('nauja', 'naudota') NOT NULL,
	ID_ integer AUTO_INCREMENT,
	PRIMARY KEY(ID_)
);

CREATE TABLE GEDIMAS
(
	pavadinimas varchar (255) NOT NULL,
	tipas ENUM('kebulo gedimas', 'vaziuokles gedimas', 'variklio gedimas', 'aptarnavimas', 'kita') NOT NULL,
	ID_ integer AUTO_INCREMENT,
	PRIMARY KEY(ID_)
);

CREATE TABLE MECHANIKAS
(
	vardas varchar (255) NOT NULL,
	pavarde varchar (255) NOT NULL,
	telefonas varchar (255) NOT NULL,
	tabelio_nr integer,
	fk_SERVISASID_ integer NOT NULL,
	PRIMARY KEY(tabelio_nr),
	CONSTRAINT fkc_serviso_mechanikas FOREIGN KEY(fk_SERVISASID_) REFERENCES SERVISAS (ID_)
);
ALTER TABLE MECHANIKAS AUTO_INCREMENT = 100;

CREATE TABLE MODELIS
(
	modelio_pavadinimas varchar (255) NOT NULL,
	ID_ integer AUTO_INCREMENT,
	fk_GAMINTOJASID_ integer NOT NULL,
	PRIMARY KEY(ID_),
	CONSTRAINT fkc_gamintojas FOREIGN KEY(fk_GAMINTOJASID_) REFERENCES GAMINTOJAS (ID_)
);

CREATE TABLE AUTOMOBILIS
(
	vin varchar (255),
	valstybinis_nr varchar (255),
	pirmos_registracijos_metai integer,
	variklio_pavadinimas varchar (255),
	spalva varchar (255),
	faktine_variklio_galia varchar (255),
	pavaru_deze ENUM('automatine', 'mechanine') NOT NULL,
	kebulas ENUM('sedanas', 'hecbekas', 'universalas', 'coupe', 'SUV', 'pikapas', 'krosoveris', 'kita') NOT NULL,
	kuro_tipas ENUM('benzinas', 'dyzelinas', 'elektra', 'elektra-benzinas', 'elektra-dyzelinas', 'kita') NOT NULL,
	fk_MODELISID_ integer NOT NULL,
	PRIMARY KEY(vin),
	CONSTRAINT fkc_modelis FOREIGN KEY(fk_MODELISID_) REFERENCES MODELIS (ID_)
);

CREATE TABLE REMONTO_SUTARTIS
(
	nr integer AUTO_INCREMENT,
	automobilio_priemimo_data timestamp,
	numatoma_suremontavimo_data date,
	pradine_rida integer NOT NULL,
	galutine_rida integer,
	remonto_kaina decimal (10, 2),
	remonto_busena ENUM('remontuojamas', 'suremontuotas', 'priimtas laukia eileje', 'atsaukta', 'atsiskaityta') NOT NULL,
	fk_AUTOMOBILISvin varchar (255) NOT NULL,
	fk_DARBUOTOJAStabelio_nr integer NOT NULL,
	fk_KLIENTASasmens_kodas varchar(255) NOT NULL,
	PRIMARY KEY(nr),
	CONSTRAINT fkc_automoblis FOREIGN KEY(fk_AUTOMOBILISvin) REFERENCES AUTOMOBILIS (vin),
	CONSTRAINT fkc_darbuotojas FOREIGN KEY(fk_DARBUOTOJAStabelio_nr) REFERENCES DARBUOTOJAS (tabelio_nr),
	CONSTRAINT fkc_klientas_sutartis FOREIGN KEY(fk_KLIENTASasmens_kodas) REFERENCES KLIENTAS (asmens_kodas)
);
ALTER TABLE REMONTO_SUTARTIS AUTO_INCREMENT = 10000;

CREATE TABLE aptarnauja
(
	fk_REMONTO_SUTARTISnr integer,
	fk_MECHANIKAStabelio_nr integer,
	PRIMARY KEY(fk_REMONTO_SUTARTISnr, fk_MECHANIKAStabelio_nr),
	CONSTRAINT fkc_aptarnauja_sutartis FOREIGN KEY(fk_REMONTO_SUTARTISnr) REFERENCES REMONTO_SUTARTIS (nr),
	CONSTRAINT fkc_mechanikas FOREIGN KEY(fk_MECHANIKAStabelio_nr) REFERENCES MECHANIKAS (tabelio_nr)
);

CREATE TABLE ATLIKTAS_DARBAS
(
	darbo_pavadinimas varchar (255) NOT NULL,
	darbo_kaina integer NOT NULL,
	bendra_kaina decimal (10, 2),
	nuvaziuota integer NOT NULL,
	ID_ integer,
	fk_REMONTO_SUTARTISnr integer NOT NULL,
	PRIMARY KEY(ID_),
	CONSTRAINT padaro FOREIGN KEY(fk_REMONTO_SUTARTISnr) REFERENCES REMONTO_SUTARTIS (nr)
);

CREATE TABLE SASKAITA
(
	nr integer AUTO_INCREMENT,
	data timestamp,
	liko decimal (10, 2),
	busena ENUM('sumoketa', 'nesumoketa'),
	fk_REMONTO_SUTARTISnr integer NOT NULL,
	PRIMARY KEY(nr),
	UNIQUE(fk_REMONTO_SUTARTISnr),
	CONSTRAINT fkc_saskaita_sutartis FOREIGN KEY(fk_REMONTO_SUTARTISnr) REFERENCES REMONTO_SUTARTIS (nr)
);
ALTER TABLE SASKAITA AUTO_INCREMENT = 1000;

CREATE TABLE turi
(
	fk_REMONTO_SUTARTISnr integer NOT NULL,
	fk_GEDIMASID_ integer NOT NULL,
	PRIMARY KEY(fk_REMONTO_SUTARTISnr, fk_GEDIMASID_),
	CONSTRAINT fkc_gedimas_sutartis FOREIGN KEY(fk_REMONTO_SUTARTISnr) REFERENCES REMONTO_SUTARTIS (nr),
	CONSTRAINT fkc_gedimas FOREIGN KEY(fk_GEDIMASID_) REFERENCES GEDIMAS (ID_)
);

CREATE TABLE DETALES_KIEKIS
(
	kiekis integer,
	bendra_kaina decimal (10, 2),
	ID_ integer AUTO_INCREMENT,
	fk_DETALEID_ integer NOT NULL,
	fk_ATLIKTAS_DARBASID_ integer NOT NULL,
	PRIMARY KEY(ID_),
	CONSTRAINT fkc_detale FOREIGN KEY(fk_DETALEID_) REFERENCES DETALE (ID_),
	CONSTRAINT fkc_darbas FOREIGN KEY(fk_ATLIKTAS_DARBASID_) REFERENCES ATLIKTAS_DARBAS (ID_)
);

CREATE TABLE MOKEJIMAS
(
	data timestamp,
	mokejimo_suma decimal (10, 2),
	ID_ integer AUTO_INCREMENT,
	fk_SASKAITAnr integer NOT NULL,
	fk_KLIENTASasmens_kodas varchar(255) NOT NULL,
	PRIMARY KEY(ID_),
	CONSTRAINT fkc_saskaita FOREIGN KEY(fk_SASKAITAnr) REFERENCES SASKAITA (nr),
	CONSTRAINT fkc_klientas FOREIGN KEY(fk_KLIENTASasmens_kodas) REFERENCES KLIENTAS (asmens_kodas)
);

DELIMITER //
CREATE TRIGGER kiekis_added
BEFORE INSERT ON DETALES_KIEKIS
FOR EACH ROW
BEGIN
    DECLARE kaina DECIMAL(10, 2);
	DECLARE darbas INT;
    
    SELECT detales_kaina INTO kaina FROM DETALE WHERE ID_ = NEW.fk_DETALEID_;

    SET NEW.bendra_kaina = kaina * NEW.kiekis;

	SELECT darbo_kaina INTO darbas FROM ATLIKTAS_DARBAS WHERE ID_ = NEW.fk_ATLIKTAS_DARBASID_;

	UPDATE ATLIKTAS_DARBAS SET bendra_kaina = NEW.bendra_kaina + darbas WHERE ID_ = NEW.fk_ATLIKTAS_DARBASID_;
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER sutartis_added
BEFORE INSERT ON REMONTO_SUTARTIS
FOR EACH ROW
BEGIN
	SET NEW.galutine_rida = NEW.pradine_rida;
	SET NEW.remonto_busena = 'priimtas laukia eileje';
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER darbas_updated
AFTER UPDATE ON ATLIKTAS_DARBAS
FOR EACH ROW
BEGIN
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina - OLD.bendra_kaina WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina + NEW.bendra_kaina WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER kiekis_deleted
BEFORE DELETE ON DETALES_KIEKIS
FOR EACH ROW
BEGIN
	DECLARE kaina DECIMAL (10, 2);

	SELECT bendra_kaina INTO kaina FROM DETALES_KIEKIS WHERE ID_ = OLD.ID_;

	UPDATE ATLIKTAS_DARBAS SET bendra_kaina = bendra_kaina - kaina WHERE ID_ = OLD.fk_ATLIKTAS_DARBASID_;
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER darbas_added
BEFORE INSERT ON ATLIKTAS_DARBAS
FOR EACH ROW
BEGIN
	SET NEW.bendra_kaina = NEW.darbo_kaina;
	UPDATE REMONTO_SUTARTIS SET galutine_rida = galutine_rida + NEW.nuvaziuota WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina + NEW.bendra_kaina WHERE nr = NEW.fk_REMONTO_SUTARTISnr; 
	UPDATE REMONTO_SUTARTIS set remonto_busena = 'remontuojamas' WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER rida_galutine_deleted
BEFORE DELETE ON ATLIKTAS_DARBAS
FOR EACH ROW
BEGIN
	DECLARE isviso integer;

	SELECT nuvaziuota INTO isviso FROM ATLIKTAS_DARBAS WHERE ID_ = OLD.ID_;

	UPDATE REMONTO_SUTARTIS SET galutine_rida = galutine_rida - isviso WHERE nr = OLD.fk_REMONTO_SUTARTISnr; 
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina - OLD.bendra_kaina WHERE nr = OLD.fk_REMONTO_SUTARTISnr; 
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER state_suremontuotas
BEFORE INSERT ON SASKAITA
FOR EACH ROW
BEGIN
	DECLARE kaina DECIMAL(10,2);
	UPDATE REMONTO_SUTARTIS SET remonto_busena = 'suremontuotas' WHERE nr = NEW.fk_REMONTO_SUTARTISnr; 

	SELECT remonto_kaina INTO kaina FROM REMONTO_SUTARTIS WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
	SET NEW.liko = kaina;
	SET NEW.busena = 'nesumoketa';
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER mokejimas
AFTER INSERT ON MOKEJIMAS
FOR EACH ROW
BEGIN
	DECLARE likutis DECIMAL(10,2);
	UPDATE SASKAITA SET liko = liko - NEW.mokejimo_suma WHERE nr = NEW.fk_SASKAITAnr;

	SELECT liko INTO likutis FROM SASKAITA WHERE nr = NEW.fk_SASKAITAnr;

	IF likutis <= 0 THEN 
	UPDATE SASKAITA SET busena = 'sumoketa' WHERE nr = NEW.fk_SASKAITAnr;
	END IF;
END;
//
DELIMITER ;
DELIMITER //
CREATE TRIGGER state_atsiskaityta
AFTER UPDATE ON SASKAITA
FOR EACH ROW
BEGIN
	UPDATE REMONTO_SUTARTIS SET remonto_busena = 'atsiskaityta' WHERE nr = nr;
END;
//
DELIMITER ;