-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Czas generowania: 02 Kwi 2021, 16:47
-- Wersja serwera: 10.4.8-MariaDB
-- Wersja PHP: 7.3.11

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Baza danych: `essentialmode`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `items`
--

CREATE TABLE `items` (
  `name` varchar(50) COLLATE utf8mb4_bin NOT NULL,
  `label` varchar(255) COLLATE utf8mb4_bin NOT NULL,
  `limit` int(11) NOT NULL DEFAULT -1,
  `rare` int(11) NOT NULL DEFAULT 0,
  `can_remove` int(11) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;

--
-- Zrzut danych tabeli `items`
--

INSERT INTO `items` (`name`, `label`, `limit`, `rare`, `can_remove`) VALUES
('fajnyheist_c4', 'C4', 1, 0, 1),
('fajnyheist_contact', 'Kontakt - napad', 1, 0, 1),
('fajnyheist_documents', 'Dokumenty rządowe', 1, 0, 1),
('fajnyheist_dustpowder', 'Proch', 1, 0, 1),
('fajnyheist_enter_loud', 'Wejściówka do bazy', 1, 0, 1),
('fajnyheist_enter_silent', 'Wejściówka do bazy', 1, 0, 1),
('fajnyheist_hackingtool', 'Urządzenie hakerskie', 1, 0, 1);

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`name`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
