-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 25, 2024 at 09:18 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `lab1_autoremontas`
--

-- --------------------------------------------------------

--
-- Table structure for table `aptarnauja`
--

CREATE TABLE `aptarnauja` (
  `fk_REMONTO_SUTARTISnr` int(11) NOT NULL,
  `fk_MECHANIKAStabelio_nr` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `aptarnauja`
--

INSERT INTO `aptarnauja` (`fk_REMONTO_SUTARTISnr`, `fk_MECHANIKAStabelio_nr`) VALUES
(10014, 102),
(10014, 103),
(10015, 101),
(10015, 102),
(10015, 103),
(10017, 100),
(10018, 101),
(10019, 102),
(10020, 103),
(10021, 105),
(10026, 102),
(10030, 101),
(10031, 103),
(10032, 103);

-- --------------------------------------------------------

--
-- Table structure for table `atliktas_darbas`
--

CREATE TABLE `atliktas_darbas` (
  `darbo_pavadinimas` varchar(255) NOT NULL,
  `darbo_kaina` int(11) NOT NULL,
  `bendra_kaina` decimal(10,2) DEFAULT NULL,
  `nuvaziuota` int(11) NOT NULL,
  `ID_` int(11) NOT NULL,
  `fk_REMONTO_SUTARTISnr` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `atliktas_darbas`
--

INSERT INTO `atliktas_darbas` (`darbo_pavadinimas`, `darbo_kaina`, `bendra_kaina`, `nuvaziuota`, `ID_`, `fk_REMONTO_SUTARTISnr`) VALUES
('Pakeistos zvakes, generatorius', 160, 190.00, 69, 8, 10017),
('Pakeista variklio grandinė', 300, 507.95, 140, 9, 10014),
('Engine repair', 200, 200.00, 50, 15, 10017),
('Brake replacement', 150, 150.00, 30, 16, 10018),
('Oil change', 50, 50.00, 10, 17, 10019),
('Suspension check', 100, 100.00, 20, 18, 10020),
('Electrical system diagnostics', 120, 120.00, 25, 19, 10021),
('asf', 10, 74.00, 0, 53, 10031),
('pakesita', 15, 196.00, 0, 54, 10022),
('Pakeistas Obadas', 80, 261.00, 116, 55, 10029),
('pk', 60, 240.00, 20, 95, 10026);

--
-- Triggers `atliktas_darbas`
--
DELIMITER $$
CREATE TRIGGER `darbas_added` BEFORE INSERT ON `atliktas_darbas` FOR EACH ROW BEGIN
	SET NEW.bendra_kaina = NEW.darbo_kaina;
	UPDATE REMONTO_SUTARTIS SET galutine_rida = galutine_rida + NEW.nuvaziuota WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina + NEW.bendra_kaina WHERE nr = NEW.fk_REMONTO_SUTARTISnr; 
	UPDATE REMONTO_SUTARTIS set remonto_busena = 'remontuojamas' WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `darbas_deleted` BEFORE DELETE ON `atliktas_darbas` FOR EACH ROW BEGIN
	DECLARE isviso integer;

	SELECT nuvaziuota INTO isviso FROM ATLIKTAS_DARBAS WHERE ID_ = OLD.ID_;

	UPDATE REMONTO_SUTARTIS SET galutine_rida = galutine_rida - isviso WHERE nr = OLD.fk_REMONTO_SUTARTISnr; 
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina - OLD.bendra_kaina WHERE nr = OLD.fk_REMONTO_SUTARTISnr; 
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `darbas_updated` AFTER UPDATE ON `atliktas_darbas` FOR EACH ROW BEGIN
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina - OLD.bendra_kaina WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
	UPDATE REMONTO_SUTARTIS SET remonto_kaina = remonto_kaina + NEW.bendra_kaina WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `automobilis`
--

CREATE TABLE `automobilis` (
  `valstybinis_nr` varchar(6) DEFAULT NULL,
  `vin` varchar(255) NOT NULL,
  `pirmos_registracijos_metai` year(4) DEFAULT NULL,
  `variklio_pavadinimas` varchar(255) DEFAULT NULL,
  `spalva` varchar(255) DEFAULT NULL,
  `faktine_variklio_galia` varchar(255) DEFAULT NULL,
  `pavaru_deze` enum('automatine','mechanine') DEFAULT NULL,
  `kebulas` enum('sedanas','hecbekas','universalas','coupe','SUV','pikapas','krosoveris','kita') DEFAULT NULL,
  `kuro_tipas` enum('benzinas','dyzelinas','elektra','elektra-benzinas','elektra-dyzelinas','kita') DEFAULT NULL,
  `fk_MODELISID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `automobilis`
--

INSERT INTO `automobilis` (`valstybinis_nr`, `vin`, `pirmos_registracijos_metai`, `variklio_pavadinimas`, `spalva`, `faktine_variklio_galia`, `pavaru_deze`, `kebulas`, `kuro_tipas`, `fk_MODELISID_`) VALUES
('213442', '123', '2016', '123', 'gelton', '123', 'mechanine', 'universalas', 'elektra-benzinas', 17),
('MNO345', '1G1FB1RX8G0123456', '2016', 'LUK', 'zalia', '150 kW', 'automatine', 'sedanas', 'dyzelinas', 5),
('JKL012', '2HGFG3B56FH503743', '2015', 'R18A1', 'melyna', '110 kW', 'mechanine', 'coupe', 'benzinas', 4),
('DEF456', 'JM1CW2BL0E0112345', '2014', 'MZR-CD', 'raudona', '100 kW', 'mechanine', 'hecbekas', 'dyzelinas', 2),
('CBR186', 'OPELZOPEL', '2001', 'ECOTEC', 'Zalia', '96 kW', 'mechanine', 'universalas', 'dyzelinas', 15),
('GHI789', 'WAUZZZ8K9BA011234', '2011', 'CAEB', 'balta', '155 kW', 'automatine', 'universalas', 'benzinas', 3),
('ABC123', 'WBA3B9G56ENR90821', '2016', 'N47D20', 'juoda', '120 kW', 'automatine', 'sedanas', 'dyzelinas', 1),
('LKA854', 'WBSWD93549PY43903', '2009', 'N47B20', 'Sidabrinė', '120kw', 'mechanine', 'sedanas', 'dyzelinas', 1),
('ABC123', 'YS2R4X20005399401', '2018', 'Engine1', 'Red', 'Power1', 'automatine', 'sedanas', 'benzinas', 1),
('DEF456', 'YS2R4X20005399402', '2019', 'Engine2', 'Blue', 'Power2', 'mechanine', 'hecbekas', 'dyzelinas', 2),
('GHI789', 'YS2R4X20005399403', '2020', 'Engine3', 'Green', 'Power3', 'automatine', 'universalas', 'elektra', 3),
('JKL012', 'YS2R4X20005399404', '2017', 'Engine4', 'Yellow', 'Power4', 'mechanine', 'SUV', 'elektra-benzinas', 4),
('MNO345', 'YS2R4X20005399405', '2016', 'Engine5', 'Black', 'Power5', 'automatine', 'coupe', 'dyzelinas', 5);

-- --------------------------------------------------------

--
-- Table structure for table `darbuotojas`
--

CREATE TABLE `darbuotojas` (
  `vardas` varchar(255) DEFAULT NULL,
  `pavarde` varchar(255) DEFAULT NULL,
  `tabelio_nr` int(11) NOT NULL,
  `telefonas` varchar(255) DEFAULT NULL,
  `fk_SERVISASID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `darbuotojas`
--

INSERT INTO `darbuotojas` (`vardas`, `pavarde`, `tabelio_nr`, `telefonas`, `fk_SERVISASID_`) VALUES
('Vardenis', 'Pavardenis', 100, '+37061234567', 1),
('Algirdas', 'Petrauskas', 101, '+37069987654', 1),
('Živilė', 'Jankauskaitė', 102, '+37068765432', 1),
('Gintaras', 'Kazlauskas', 103, '+37065432109', 1),
('Adomas', 'Jonaitis', 104, '+37067890123', 1);

-- --------------------------------------------------------

--
-- Table structure for table `detale`
--

CREATE TABLE `detale` (
  `pavadinimas` varchar(255) DEFAULT NULL,
  `detales_kaina` decimal(10,2) DEFAULT NULL,
  `detales_bukle` enum('nauja','naudota') DEFAULT NULL,
  `ID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `detale`
--

INSERT INTO `detale` (`pavadinimas`, `detales_kaina`, `detales_bukle`, `ID_`) VALUES
('Opel Stabdžių diskas', 80.51, 'nauja', 1),
('BMW E9x PRE-FL žibintas', 85.43, 'naudota', 2),
('Degimo žvakė NGK BR9HS', 4.99, 'nauja', 3),
('H4 HELLA lemputė', 65.75, 'nauja', 4),
('10W40 Castrol variklio tepalas 1L', 15.00, 'nauja', 5),
('Kuro filtras', 9.45, 'nauja', 6),
('Variklio N47B20 paskirstymo grandinės kompl.', 207.95, 'nauja', 7),
('Alyvos filtras M50B25', 12.75, 'nauja', 8),
('Kuro pompa Opel Zafira', 15.00, 'naudota', 9),
('Akumuliatorius 80Ah', 90.00, 'nauja', 10),
('Generatoriaus dirželis', 14.02, 'nauja', 11),
('Generatorius', 50.60, 'nauja', 12),
('Drakoninė lempa', 420.69, 'nauja', 15),
('Maitniko obadas', 181.00, 'naudota', 16),
('Karbo propkė', 1000.00, 'naudota', 17),
('Aprilia SR su atsisukusias stabdžių disko varžtais', 1430.00, 'naudota', 18),
('Paauksuoti varžtai 999 praba', 10.69, NULL, 19),
('Paauksuoti varžtai (lieva praba)', 500.00, 'nauja', 20),
('Babina', 64.00, 'nauja', 21),
('Variklio pistonas', 150.00, 'nauja', 22),
('Stabdžių trinkelės', 80.00, 'nauja', 23),
('Kuro filtras', 25.00, 'nauja', 24),
('Žvakė', 10.00, 'nauja', 25),
('Alyvos filtras', 15.00, 'nauja', 26),
('Oro filtras', 20.00, 'nauja', 27),
('Radiatorių', 200.00, 'nauja', 28),
('Baterija', 100.00, 'nauja', 29),
('Generatorius', 150.00, 'nauja', 30),
('Starteris', 120.00, 'nauja', 31),
('Termostatas', 30.00, 'nauja', 32),
('Vandens pompa', 80.00, 'nauja', 33),
('Diržo', 50.00, 'nauja', 34),
('Sankabos komplektas', 200.00, 'nauja', 35),
('Stabdžių amortizatorius', 80.00, 'nauja', 36),
('Stūmoklio kreivasis', 150.00, 'nauja', 37),
('Stipinų galas', 30.00, 'nauja', 38),
('Ratų guolis', 40.00, 'nauja', 39),
('Tasiko sąnaris', 60.00, 'nauja', 40),
('Apsauginės ritės', 40.00, 'nauja', 41);

-- --------------------------------------------------------

--
-- Table structure for table `detales_kiekis`
--

CREATE TABLE `detales_kiekis` (
  `kiekis` int(11) DEFAULT NULL,
  `bendra_kaina` decimal(10,2) DEFAULT NULL,
  `ID_` int(11) NOT NULL,
  `fk_DETALEID_` int(11) NOT NULL,
  `fk_ATLIKTAS_DARBASID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `detales_kiekis`
--

INSERT INTO `detales_kiekis` (`kiekis`, `bendra_kaina`, `ID_`, `fk_DETALEID_`, `fk_ATLIKTAS_DARBASID_`) VALUES
(4, 19.96, 5, 3, 8),
(1, 50.60, 6, 12, 8),
(1, 207.95, 7, 7, 9),
(2, 161.02, 28, 1, 8),
(2, 30.00, 41, 9, 8),
(1, 64.00, 72, 21, 53),
(1, 181.00, 73, 16, 54),
(1, 181.00, 74, 16, 55),
(2, 180.00, 114, 10, 95);

--
-- Triggers `detales_kiekis`
--
DELIMITER $$
CREATE TRIGGER `kiekis_added` BEFORE INSERT ON `detales_kiekis` FOR EACH ROW BEGIN
    DECLARE kaina DECIMAL(10, 2);
	DECLARE darbas INT;
    
    SELECT detales_kaina INTO kaina FROM DETALE WHERE ID_ = NEW.fk_DETALEID_;

    SET NEW.bendra_kaina = kaina * NEW.kiekis;

	SELECT darbo_kaina INTO darbas FROM ATLIKTAS_DARBAS WHERE ID_ = NEW.fk_ATLIKTAS_DARBASID_;

	UPDATE ATLIKTAS_DARBAS SET bendra_kaina = NEW.bendra_kaina + darbas WHERE ID_ = NEW.fk_ATLIKTAS_DARBASID_;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `kiekis_deleted` BEFORE DELETE ON `detales_kiekis` FOR EACH ROW BEGIN
	DECLARE kaina DECIMAL (10, 2);

	SELECT bendra_kaina INTO kaina FROM DETALES_KIEKIS WHERE ID_ = OLD.ID_;

	UPDATE ATLIKTAS_DARBAS SET bendra_kaina = bendra_kaina - kaina WHERE ID_ = OLD.fk_ATLIKTAS_DARBASID_;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `gamintojas`
--

CREATE TABLE `gamintojas` (
  `pavadinimas` varchar(255) DEFAULT NULL,
  `ID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `gamintojas`
--

INSERT INTO `gamintojas` (`pavadinimas`, `ID_`) VALUES
('Toyota', 1),
('Subaru', 2),
('Mitsubishi', 3),
('Volkswagen', 4),
('Ford', 5),
('Honda', 6),
('BMW', 7),
('Mercedes-Benz', 8),
('Audi', 9),
('Renault', 10),
('Citroen', 11),
('Hyundai', 12),
('Nissan', 13),
('Chevrolet', 14),
('Opel', 15),
('Brabus', 18),
('Techart', 19),
('Alpina', 20),
('Ferrari', 21),
('Lamborghini', 22),
('Aston Martin', 23),
('Lancia', 24),
('Sauber', 25),
('Dacia', 26),
('McLaren', 27),
('Saab', 28),
('Fiat', 29),
('Datsun', 30);

-- --------------------------------------------------------

--
-- Table structure for table `gedimas`
--

CREATE TABLE `gedimas` (
  `pavadinimas` varchar(255) DEFAULT NULL,
  `tipas` enum('kebulo gedimas','vaziuokles gedimas','variklio gedimas','elektros gedimas','aptarnavimas','kita') DEFAULT NULL,
  `ID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `gedimas`
--

INSERT INTO `gedimas` (`pavadinimas`, `tipas`, `ID_`) VALUES
('Neharmoningas vidaus degimas', 'variklio gedimas', 3),
('Kratymasis', 'vaziuokles gedimas', 4),
('Parktonikai neveikia', 'elektros gedimas', 7),
('Stiklo įtrūkimas', 'kebulo gedimas', 17),
('Reguliavimas', 'aptarnavimas', 20),
('Žibintų keitimas', 'elektros gedimas', 21),
('Kondicionieriaus sistemos gedimas', 'kita', 23),
('Važiuoklės reguliavimas', 'vaziuokles gedimas', 24),
('Kuro filtro keitimas', 'variklio gedimas', 25),
('Variklio aptarnavimas', 'aptarnavimas', 26),
('Kuro sistemos aptarnavimas', 'aptarnavimas', 27),
('Paskirstymo grandinių keitimas', 'variklio gedimas', 28),
('Pašildymo žvakių keitimas', 'variklio gedimas', 29),
('Paskirstymo dirželio keitimas', 'variklio gedimas', 30),
('CHECK ENGINE', 'variklio gedimas', 31),
('Uždegimo žvakių keitimas', 'variklio gedimas', 32),
('Maitniko obadas atšoko.', 'elektros gedimas', 33),
('Variklio trūkčiojimas', 'variklio gedimas', 34),
('Reguliarus aptarnavimas', 'aptarnavimas', 35);

-- --------------------------------------------------------

--
-- Table structure for table `klientas`
--

CREATE TABLE `klientas` (
  `asmens_kodas` varchar(11) NOT NULL,
  `vardas` varchar(255) DEFAULT NULL,
  `pavarde` varchar(255) DEFAULT NULL,
  `telefonas` varchar(255) DEFAULT NULL,
  `epastas` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `klientas`
--

INSERT INTO `klientas` (`asmens_kodas`, `vardas`, `pavarde`, `telefonas`, `epastas`) VALUES
('01234567890', 'Algirdas', 'Algirdaitis', '+37061234576', 'algirdas.algirdaitis@example.com'),
('03579246801', 'Rasa', 'Rasutė', '+37061234586', 'rasa.rasute@example.com'),
('12345678901', 'Jonas', 'Petraitis', '+37061234567', 'jonas@gmail.com'),
('13579246810', 'Eglė', 'Eglėnaitė', '+37061234577', 'egle.eglenaite@example.com'),
('23456789012', 'Petras', 'Petraitis', '+37061234568', 'petras.petraitis@example.com'),
('24680135792', 'Simas', 'Simaitis', '+37061234578', 'simas.simaitis@example.com'),
('34567890123', 'Antanas', 'Antanaitis', '+37061234569', 'antanas.antanaitis@example.com'),
('35792468013', 'Dalia', 'Daliauskaitė', '+37061234579', 'dalia.daliauskaite@example.com'),
('45678901234', 'Mindaugas', 'Mindaugaitis', '+37061234570', 'mindaugas.mindaugaitis@example.com'),
('46801357924', 'Ramūnas', 'Ramūnas', '+37061234580', 'ramunas.ramunas@example.com'),
('55555555555', 'Antanas', 'Garšva', '+37068765432', 'antanas@yahoo.com'),
('56789012345', 'Jurga', 'Jurgaitė', '+37061234571', 'jurga.jurgaite@example.com'),
('57924680135', 'Viktorija', 'Viktorija', '+37061234581', 'viktorija.viktorija@example.com'),
('67890123456', 'Giedrė', 'Giedraitytė', '+37061234572', 'giedre.giedraityte@example.com'),
('68013579246', 'Aurimas', 'Aurimaitis', '+37061234582', 'aurimas.aurimaitis@example.com'),
('77777777777', 'Marytė', 'Marytėnaitė', '+37067890123', 'maryte@nvidia.com'),
('78901234567', 'Rūta', 'Rutkauskaitė', '+37061234573', 'ruta.rutkauskaite@example.com'),
('79135792468', 'Greta', 'Gretaitė', '+37061234583', 'greta.gretaitis@example.com'),
('80135792468', 'Marius', 'Mariūnas', '+37061234584', 'marius.mariunas@example.com'),
('88888888888', 'Ona', 'Onaitė', '+37065432109', 'ona@inoo3D.com'),
('89012345678', 'Lukas', 'Lukauskas', '+37061234574', 'lukas.lukauskas@example.com'),
('90123456789', 'Gabija', 'Gabijauskaitė', '+37061234575', 'gabija.gabijauskaite@example.com'),
('92468013579', 'Inga', 'Ingaitytė', '+37061234585', 'inga.ingaite@example.com'),
('98765432109', 'Petras', 'Trečiokas', '+37069876543', 'petras@gmail.com');

-- --------------------------------------------------------

--
-- Table structure for table `mechanikas`
--

CREATE TABLE `mechanikas` (
  `vardas` varchar(255) DEFAULT NULL,
  `pavarde` varchar(255) DEFAULT NULL,
  `telefonas` varchar(255) DEFAULT NULL,
  `tabelio_nr` int(11) NOT NULL,
  `fk_SERVISASID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `mechanikas`
--

INSERT INTO `mechanikas` (`vardas`, `pavarde`, `telefonas`, `tabelio_nr`, `fk_SERVISASID_`) VALUES
('Algirdas', 'Jankauskas', '+37060009876', 100, 1),
('Artūras', 'Butkus', '+37060009876', 101, 1),
('Žygimantas', 'Kazlauskas', '+37060007890', 102, 1),
('Jonas', 'Adomaitis', '+37060005432', 103, 1),
('Kazimieras', 'Jonaitis', '+37060001234', 105, 1),
('Vytautas', 'Petrauskas', '+37060004567', 111, 1);

-- --------------------------------------------------------

--
-- Table structure for table `modelis`
--

CREATE TABLE `modelis` (
  `modelio_pavadinimas` varchar(255) DEFAULT NULL,
  `ID_` int(11) NOT NULL,
  `fk_GAMINTOJASID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `modelis`
--

INSERT INTO `modelis` (`modelio_pavadinimas`, `ID_`, `fk_GAMINTOJASID_`) VALUES
('320', 1, 7),
('325', 2, 7),
('330', 3, 7),
('335', 4, 7),
('M3', 5, 7),
('520', 6, 7),
('525', 7, 7),
('530', 8, 7),
('535', 9, 7),
('540', 10, 7),
('M5', 11, 7),
('Camry', 12, 1),
('Fiesta', 13, 5),
('Focus', 14, 5),
('Zafira', 15, 15),
('Kangoo', 16, 10),
('Civic', 17, 6),
('Accord', 18, 6),
('NSX', 19, 6),
('Jazz', 20, 6),
('Prelude', 21, 6),
('Corsa', 22, 15),
('Rekord', 23, 15),
('Astra', 24, 15),
('Omega', 25, 15),
('A1', 26, 9),
('A3', 27, 9),
('A4', 28, 9),
('A5', 29, 9),
('A6', 30, 9),
('A7', 31, 9),
('A8', 32, 9);

-- --------------------------------------------------------

--
-- Table structure for table `mokejimas`
--

CREATE TABLE `mokejimas` (
  `data` date DEFAULT NULL,
  `mokejimo_suma` int(11) DEFAULT NULL,
  `ID_` int(11) NOT NULL,
  `fk_KLIENTASasmens_kodas` varchar(255) NOT NULL,
  `fk_SASKAITAnr` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `mokejimas`
--

INSERT INTO `mokejimas` (`data`, `mokejimo_suma`, `ID_`, `fk_KLIENTASasmens_kodas`, `fk_SASKAITAnr`) VALUES
('2024-03-04', 510, 3, '12345678901', 1003),
('2024-03-04', 510, 5, '12345678901', 1003);

--
-- Triggers `mokejimas`
--
DELIMITER $$
CREATE TRIGGER `mokejimas` AFTER INSERT ON `mokejimas` FOR EACH ROW BEGIN
	DECLARE likutis DECIMAL(10,2);
	UPDATE SASKAITA SET liko = liko - NEW.mokejimo_suma WHERE nr = NEW.fk_SASKAITAnr;

	SELECT liko INTO likutis FROM SASKAITA WHERE nr = NEW.fk_SASKAITAnr;

	IF likutis <= 0 THEN 
	UPDATE SASKAITA SET busena = 'sumoketa' WHERE nr = NEW.fk_SASKAITAnr;
	END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `remonto_sutartis`
--

CREATE TABLE `remonto_sutartis` (
  `fk_AUTOMOBILISvin` varchar(255) NOT NULL,
  `automobilio_priemimo_data` date NOT NULL,
  `nr` int(11) NOT NULL,
  `numatoma_suremontavimo_data` date DEFAULT NULL,
  `galutine_rida` int(11) DEFAULT NULL,
  `pradine_rida` int(11) DEFAULT NULL,
  `remonto_kaina` decimal(10,2) DEFAULT NULL,
  `remonto_busena` enum('remontuojamas','suremontuotas','priimtas laukia eileje','atsaukta','atsiskaityta') DEFAULT NULL,
  `fk_KLIENTASasmens_kodas` varchar(255) NOT NULL,
  `fk_DARBUOTOJAStabelio_nr` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `remonto_sutartis`
--

INSERT INTO `remonto_sutartis` (`fk_AUTOMOBILISvin`, `automobilio_priemimo_data`, `nr`, `numatoma_suremontavimo_data`, `galutine_rida`, `pradine_rida`, `remonto_kaina`, `remonto_busena`, `fk_KLIENTASasmens_kodas`, `fk_DARBUOTOJAStabelio_nr`) VALUES
('WBA3B9G56ENR90821', '2024-03-09', 10014, NULL, 50140, 50000, 507.95, 'atsiskaityta', '88888888888', 102),
('WBA3B9G56ENR90821', '2024-03-04', 10015, '2024-03-30', 145675, 145675, 0.00, 'atsiskaityta', '88888888888', 104),
('WBA3B9G56ENR90821', '2024-03-06', 10017, NULL, 1119, 1000, 190.00, 'suremontuotas', '98765432109', 102),
('YS2R4X20005399401', '2024-03-07', 10018, '2024-03-08', 50030, 50000, 150.00, 'suremontuotas', '12345678901', 100),
('YS2R4X20005399402', '2024-03-08', 10019, '2024-03-09', 60010, 60000, 50.00, 'suremontuotas', '23456789012', 101),
('YS2R4X20005399403', '2024-03-09', 10020, '2024-03-10', 70020, 70000, 100.00, 'suremontuotas', '34567890123', 102),
('YS2R4X20005399404', '2024-03-10', 10021, '2024-03-11', 80025, 80000, 120.00, 'suremontuotas', '45678901234', 103),
('YS2R4X20005399405', '2024-03-11', 10022, '2024-03-12', 90000, 90000, 196.00, 'remontuojamas', '56789012345', 104),
('OPELZOPEL', '2024-04-24', 10023, '2024-05-24', 200000, 200000, NULL, 'remontuojamas', '13579246810', 102),
('YS2R4X20005399402', '2024-04-24', 10026, '2024-04-11', 620, 600, 240.00, 'remontuojamas', '13579246810', 100),
('YS2R4X20005399403', '2024-04-24', 10029, '0001-01-01', 89116, 89000, 261.00, 'remontuojamas', '03579246801', 102),
('WAUZZZ8K9BA011234', '2024-04-24', 10030, '0001-01-01', 160, 160, 0.00, 'remontuojamas', '24680135792', 102),
('OPELZOPEL', '2024-04-24', 10031, '0001-01-01', 148424, 148424, 74.00, 'remontuojamas', '12345678901', 100),
('2HGFG3B56FH503743', '2024-04-12', 10032, '1901-08-01', 16600, 16600, 0.00, 'remontuojamas', '13579246810', 101);

--
-- Triggers `remonto_sutartis`
--
DELIMITER $$
CREATE TRIGGER `rida_update` BEFORE INSERT ON `remonto_sutartis` FOR EACH ROW BEGIN
	SET NEW.galutine_rida = NEW.pradine_rida;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `saskaita`
--

CREATE TABLE `saskaita` (
  `nr` int(11) NOT NULL,
  `data` date DEFAULT current_timestamp(),
  `busena` enum('sumoketa','nesumoketa') NOT NULL,
  `liko` decimal(10,2) DEFAULT NULL,
  `fk_REMONTO_SUTARTISnr` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `saskaita`
--

INSERT INTO `saskaita` (`nr`, `data`, `busena`, `liko`, `fk_REMONTO_SUTARTISnr`) VALUES
(1003, '2024-03-04', 'sumoketa', -512.05, 10014),
(1004, '2024-03-09', 'nesumoketa', 190.00, 10017),
(1005, '2024-03-10', 'nesumoketa', 150.00, 10018),
(1006, '2024-03-11', 'nesumoketa', 50.00, 10019),
(1007, '2024-03-12', 'nesumoketa', 100.00, 10020),
(1008, '2024-03-13', 'nesumoketa', 120.00, 10021);

--
-- Triggers `saskaita`
--
DELIMITER $$
CREATE TRIGGER `state_atsiskaityta` AFTER UPDATE ON `saskaita` FOR EACH ROW BEGIN
	UPDATE REMONTO_SUTARTIS SET remonto_busena = 'atsiskaityta' WHERE nr = nr;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `state_suremontuotas` BEFORE INSERT ON `saskaita` FOR EACH ROW BEGIN
	DECLARE kaina DECIMAL(10,2);
	UPDATE REMONTO_SUTARTIS SET remonto_busena = 'suremontuotas' WHERE nr = NEW.fk_REMONTO_SUTARTISnr; 

	SELECT remonto_kaina INTO kaina FROM REMONTO_SUTARTIS WHERE nr = NEW.fk_REMONTO_SUTARTISnr;
	SET NEW.liko = kaina;
	SET NEW.busena = 'nesumoketa';
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `servisas`
--

CREATE TABLE `servisas` (
  `pavadinimas` varchar(255) DEFAULT NULL,
  `telefonas` varchar(255) DEFAULT NULL,
  `epastas` varchar(255) DEFAULT NULL,
  `miestas` varchar(255) DEFAULT NULL,
  `gatve` varchar(255) DEFAULT NULL,
  `pastato_nr` varchar(255) DEFAULT NULL,
  `pasto_kodas` int(11) DEFAULT NULL,
  `ID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `servisas`
--

INSERT INTO `servisas` (`pavadinimas`, `telefonas`, `epastas`, `miestas`, `gatve`, `pastato_nr`, `pasto_kodas`, `ID_`) VALUES
('UAB \"Starteris\"', '+37064894000', 'kaunas@starteris.lt', 'Kaunas', 'Savanorių pr.', '98', 44147, 1);

-- --------------------------------------------------------

--
-- Table structure for table `turi`
--

CREATE TABLE `turi` (
  `fk_REMONTO_SUTARTISnr` int(11) NOT NULL,
  `fk_GEDIMASID_` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_ci;

--
-- Dumping data for table `turi`
--

INSERT INTO `turi` (`fk_REMONTO_SUTARTISnr`, `fk_GEDIMASID_`) VALUES
(10014, 3),
(10014, 31),
(10015, 4),
(10017, 3),
(10017, 25),
(10018, 26),
(10019, 27),
(10020, 28),
(10021, 29),
(10026, 29),
(10031, 31),
(10032, 21);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `aptarnauja`
--
ALTER TABLE `aptarnauja`
  ADD PRIMARY KEY (`fk_REMONTO_SUTARTISnr`,`fk_MECHANIKAStabelio_nr`),
  ADD KEY `fkc_mechanikas` (`fk_MECHANIKAStabelio_nr`);

--
-- Indexes for table `atliktas_darbas`
--
ALTER TABLE `atliktas_darbas`
  ADD PRIMARY KEY (`ID_`),
  ADD KEY `fkc_darbas_sutartis` (`fk_REMONTO_SUTARTISnr`);

--
-- Indexes for table `automobilis`
--
ALTER TABLE `automobilis`
  ADD PRIMARY KEY (`vin`),
  ADD KEY `fkc_modelis` (`fk_MODELISID_`);

--
-- Indexes for table `darbuotojas`
--
ALTER TABLE `darbuotojas`
  ADD PRIMARY KEY (`tabelio_nr`),
  ADD KEY `fkc_serviso_darbuotojas` (`fk_SERVISASID_`);

--
-- Indexes for table `detale`
--
ALTER TABLE `detale`
  ADD PRIMARY KEY (`ID_`);

--
-- Indexes for table `detales_kiekis`
--
ALTER TABLE `detales_kiekis`
  ADD PRIMARY KEY (`ID_`),
  ADD KEY `fkc_detale` (`fk_DETALEID_`),
  ADD KEY `fkc_darbas` (`fk_ATLIKTAS_DARBASID_`);

--
-- Indexes for table `gamintojas`
--
ALTER TABLE `gamintojas`
  ADD PRIMARY KEY (`ID_`);

--
-- Indexes for table `gedimas`
--
ALTER TABLE `gedimas`
  ADD PRIMARY KEY (`ID_`);

--
-- Indexes for table `klientas`
--
ALTER TABLE `klientas`
  ADD PRIMARY KEY (`asmens_kodas`);

--
-- Indexes for table `mechanikas`
--
ALTER TABLE `mechanikas`
  ADD PRIMARY KEY (`tabelio_nr`),
  ADD KEY `fkc_serviso_mechanikas` (`fk_SERVISASID_`);

--
-- Indexes for table `modelis`
--
ALTER TABLE `modelis`
  ADD PRIMARY KEY (`ID_`),
  ADD KEY `fkc_gamintojas` (`fk_GAMINTOJASID_`);

--
-- Indexes for table `mokejimas`
--
ALTER TABLE `mokejimas`
  ADD PRIMARY KEY (`ID_`),
  ADD KEY `fkc_klientas` (`fk_KLIENTASasmens_kodas`),
  ADD KEY `fkc_saskaita` (`fk_SASKAITAnr`);

--
-- Indexes for table `remonto_sutartis`
--
ALTER TABLE `remonto_sutartis`
  ADD PRIMARY KEY (`nr`),
  ADD KEY `fkc_automobilis` (`fk_AUTOMOBILISvin`),
  ADD KEY `fkc_darbuotojas` (`fk_DARBUOTOJAStabelio_nr`),
  ADD KEY `fkc_klientas_sutartis` (`fk_KLIENTASasmens_kodas`);

--
-- Indexes for table `saskaita`
--
ALTER TABLE `saskaita`
  ADD PRIMARY KEY (`nr`),
  ADD UNIQUE KEY `fk_REMONTO_SUTARTISnr` (`fk_REMONTO_SUTARTISnr`);

--
-- Indexes for table `servisas`
--
ALTER TABLE `servisas`
  ADD PRIMARY KEY (`ID_`);

--
-- Indexes for table `turi`
--
ALTER TABLE `turi`
  ADD PRIMARY KEY (`fk_REMONTO_SUTARTISnr`,`fk_GEDIMASID_`),
  ADD KEY `fkc_gedimas` (`fk_GEDIMASID_`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `atliktas_darbas`
--
ALTER TABLE `atliktas_darbas`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=96;

--
-- AUTO_INCREMENT for table `darbuotojas`
--
ALTER TABLE `darbuotojas`
  MODIFY `tabelio_nr` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=112;

--
-- AUTO_INCREMENT for table `detale`
--
ALTER TABLE `detale`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=42;

--
-- AUTO_INCREMENT for table `detales_kiekis`
--
ALTER TABLE `detales_kiekis`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=115;

--
-- AUTO_INCREMENT for table `gamintojas`
--
ALTER TABLE `gamintojas`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT for table `gedimas`
--
ALTER TABLE `gedimas`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=36;

--
-- AUTO_INCREMENT for table `mechanikas`
--
ALTER TABLE `mechanikas`
  MODIFY `tabelio_nr` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=112;

--
-- AUTO_INCREMENT for table `modelis`
--
ALTER TABLE `modelis`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- AUTO_INCREMENT for table `mokejimas`
--
ALTER TABLE `mokejimas`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT for table `remonto_sutartis`
--
ALTER TABLE `remonto_sutartis`
  MODIFY `nr` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10033;

--
-- AUTO_INCREMENT for table `saskaita`
--
ALTER TABLE `saskaita`
  MODIFY `nr` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1009;

--
-- AUTO_INCREMENT for table `servisas`
--
ALTER TABLE `servisas`
  MODIFY `ID_` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `aptarnauja`
--
ALTER TABLE `aptarnauja`
  ADD CONSTRAINT `fkc_aptarnauja_sutartis` FOREIGN KEY (`fk_REMONTO_SUTARTISnr`) REFERENCES `remonto_sutartis` (`nr`),
  ADD CONSTRAINT `fkc_mechanikas` FOREIGN KEY (`fk_MECHANIKAStabelio_nr`) REFERENCES `mechanikas` (`tabelio_nr`);

--
-- Constraints for table `atliktas_darbas`
--
ALTER TABLE `atliktas_darbas`
  ADD CONSTRAINT `fkc_darbas_sutartis` FOREIGN KEY (`fk_REMONTO_SUTARTISnr`) REFERENCES `remonto_sutartis` (`nr`);

--
-- Constraints for table `automobilis`
--
ALTER TABLE `automobilis`
  ADD CONSTRAINT `fkc_modelis` FOREIGN KEY (`fk_MODELISID_`) REFERENCES `modelis` (`ID_`);

--
-- Constraints for table `darbuotojas`
--
ALTER TABLE `darbuotojas`
  ADD CONSTRAINT `fkc_serviso_darbuotojas` FOREIGN KEY (`fk_SERVISASID_`) REFERENCES `servisas` (`ID_`);

--
-- Constraints for table `detales_kiekis`
--
ALTER TABLE `detales_kiekis`
  ADD CONSTRAINT `fkc_darbas` FOREIGN KEY (`fk_ATLIKTAS_DARBASID_`) REFERENCES `atliktas_darbas` (`ID_`),
  ADD CONSTRAINT `fkc_detale` FOREIGN KEY (`fk_DETALEID_`) REFERENCES `detale` (`ID_`);

--
-- Constraints for table `mechanikas`
--
ALTER TABLE `mechanikas`
  ADD CONSTRAINT `fkc_serviso_mechanikas` FOREIGN KEY (`fk_SERVISASID_`) REFERENCES `servisas` (`ID_`);

--
-- Constraints for table `modelis`
--
ALTER TABLE `modelis`
  ADD CONSTRAINT `fkc_gamintojas` FOREIGN KEY (`fk_GAMINTOJASID_`) REFERENCES `gamintojas` (`ID_`);

--
-- Constraints for table `mokejimas`
--
ALTER TABLE `mokejimas`
  ADD CONSTRAINT `fkc_klientas` FOREIGN KEY (`fk_KLIENTASasmens_kodas`) REFERENCES `klientas` (`asmens_kodas`),
  ADD CONSTRAINT `fkc_saskaita` FOREIGN KEY (`fk_SASKAITAnr`) REFERENCES `saskaita` (`nr`);

--
-- Constraints for table `remonto_sutartis`
--
ALTER TABLE `remonto_sutartis`
  ADD CONSTRAINT `fkc_automobilis` FOREIGN KEY (`fk_AUTOMOBILISvin`) REFERENCES `automobilis` (`vin`),
  ADD CONSTRAINT `fkc_darbuotojas` FOREIGN KEY (`fk_DARBUOTOJAStabelio_nr`) REFERENCES `darbuotojas` (`tabelio_nr`),
  ADD CONSTRAINT `fkc_klientas_sutartis` FOREIGN KEY (`fk_KLIENTASasmens_kodas`) REFERENCES `klientas` (`asmens_kodas`);

--
-- Constraints for table `saskaita`
--
ALTER TABLE `saskaita`
  ADD CONSTRAINT `fkc_saskaita_sutartis` FOREIGN KEY (`fk_REMONTO_SUTARTISnr`) REFERENCES `remonto_sutartis` (`nr`);

--
-- Constraints for table `turi`
--
ALTER TABLE `turi`
  ADD CONSTRAINT `fkc_gedimas` FOREIGN KEY (`fk_GEDIMASID_`) REFERENCES `gedimas` (`ID_`),
  ADD CONSTRAINT `fkc_gedimas_sutartis` FOREIGN KEY (`fk_REMONTO_SUTARTISnr`) REFERENCES `remonto_sutartis` (`nr`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
