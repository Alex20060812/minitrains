-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1:3307
-- Létrehozás ideje: 2026. Feb 10. 11:48
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `modellvasut`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `functions`
--

CREATE TABLE `functions` (
  `id` int(11) NOT NULL,
  `train_id` int(11) NOT NULL,
  `number` int(11) NOT NULL,
  `name` varchar(50) NOT NULL,
  `icon` varchar(100) DEFAULT NULL,
  `hidden` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `functions`
--

INSERT INTO `functions` (`id`, `train_id`, `number`, `name`, `icon`, `hidden`) VALUES
(1, 1, 0, 'f0', NULL, 0),
(2, 1, 1, 'f1', NULL, 0),
(3, 1, 2, 'f2', NULL, 0),
(4, 1, 3, 'f3', NULL, 0),
(5, 1, 4, 'f4', NULL, 0),
(6, 1, 5, 'f5', NULL, 0),
(7, 1, 6, 'f6', NULL, 0),
(8, 1, 7, 'f7', NULL, 0),
(9, 1, 8, 'f8', NULL, 0),
(10, 1, 9, 'f9', NULL, 0),
(11, 1, 10, 'f10', NULL, 0),
(12, 1, 11, 'f11', NULL, 0),
(13, 1, 12, 'f12', NULL, 0),
(14, 1, 13, 'f13', NULL, 0),
(15, 1, 14, 'f14', NULL, 0),
(16, 1, 15, 'f15', NULL, 0),
(17, 1, 16, 'f16', NULL, 0),
(18, 1, 17, 'f17', NULL, 0),
(19, 1, 18, 'f18', NULL, 0),
(20, 1, 19, 'f19', NULL, 0),
(21, 1, 20, 'f20', NULL, 0),
(22, 1, 21, 'f21', NULL, 0),
(23, 1, 22, 'f22', NULL, 0),
(24, 1, 23, 'f23', NULL, 0),
(25, 1, 24, 'f24', NULL, 0),
(26, 1, 25, 'f25', NULL, 0),
(27, 1, 26, 'f26', NULL, 0),
(28, 1, 27, 'f27', NULL, 0),
(29, 1, 28, 'f28', NULL, 0),
(30, 2, 0, 'f0', NULL, 0),
(31, 2, 1, 'f1', NULL, 0),
(32, 2, 2, 'f2', NULL, 0),
(33, 2, 3, 'f3', NULL, 0),
(34, 2, 4, 'f4', NULL, 0),
(35, 2, 5, 'f5', NULL, 0),
(36, 2, 6, 'f6', NULL, 0),
(37, 2, 7, 'f7', NULL, 0),
(38, 2, 8, 'f8', NULL, 0),
(39, 2, 9, 'f9', NULL, 0),
(40, 2, 10, 'f10', NULL, 0),
(41, 2, 11, 'f11', NULL, 0),
(42, 2, 12, 'f12', NULL, 0),
(43, 2, 13, 'f13', NULL, 0),
(44, 2, 14, 'f14', NULL, 0),
(45, 2, 15, 'f15', NULL, 0),
(46, 2, 16, 'f16', NULL, 0),
(47, 2, 17, 'f17', NULL, 0),
(48, 2, 18, 'f18', NULL, 0),
(49, 2, 19, 'f19', NULL, 0),
(50, 2, 20, 'f20', NULL, 0),
(51, 2, 21, 'f21', NULL, 0),
(52, 2, 22, 'f22', NULL, 0),
(53, 2, 23, 'f23', NULL, 0),
(54, 2, 24, 'f24', NULL, 0),
(55, 2, 25, 'f25', NULL, 0),
(56, 2, 26, 'f26', NULL, 0),
(57, 2, 27, 'f27', NULL, 0),
(58, 2, 28, 'f28', NULL, 0);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `functions_settings`
--

CREATE TABLE `functions_settings` (
  `id` int(11) NOT NULL,
  `function_id` int(11) NOT NULL,
  `custom_name` varchar(50) DEFAULT NULL,
  `default_state` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `functions_settings`
--

INSERT INTO `functions_settings` (`id`, `function_id`, `custom_name`, `default_state`) VALUES
(1, 1, 'f0', 1),
(2, 2, 'Motorhang', 1),
(3, 3, 'f2', 0),
(4, 4, 'f3', 0),
(5, 5, 'f4', 0),
(6, 6, 'f5', 0),
(7, 7, 'f6', 0),
(8, 8, 'f7', 0),
(9, 9, 'f8', 0),
(10, 10, 'f9', 0),
(11, 11, 'f10', 0),
(12, 12, 'f11', 0),
(13, 13, 'f12', 0),
(14, 14, 'f13', 0),
(15, 15, 'f14', 0),
(16, 16, 'f15', 0),
(17, 17, 'f16', 0),
(18, 18, 'f17', 0),
(19, 19, 'f18', 0),
(20, 20, 'f19', 0),
(21, 21, 'f20', 0),
(22, 22, 'f21', 0),
(23, 23, 'f22', 0),
(24, 24, 'f23', 0),
(25, 25, 'f24', 0),
(26, 26, 'f25', 0),
(27, 27, 'f26', 0),
(28, 28, 'f27', 0),
(29, 29, 'f28', 0),
(30, 30, NULL, 0),
(31, 31, NULL, 0),
(32, 32, NULL, 0),
(33, 33, NULL, 0),
(34, 34, NULL, 0),
(35, 35, NULL, 0),
(36, 36, NULL, 0),
(37, 37, NULL, 0),
(38, 38, NULL, 0),
(39, 39, NULL, 0),
(40, 40, NULL, 0),
(41, 41, NULL, 0),
(42, 42, NULL, 0),
(43, 43, NULL, 0),
(44, 44, NULL, 0),
(45, 45, NULL, 0),
(46, 46, NULL, 0),
(47, 47, NULL, 0),
(48, 48, NULL, 0),
(49, 49, NULL, 0),
(50, 50, NULL, 0),
(51, 51, NULL, 0),
(52, 52, NULL, 0),
(53, 53, NULL, 0),
(54, 54, NULL, 0),
(55, 55, NULL, 0),
(56, 56, NULL, 0),
(57, 57, NULL, 0),
(58, 58, NULL, 0);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `trains`
--

CREATE TABLE `trains` (
  `id` int(11) NOT NULL,
  `user_id` int(11) NOT NULL,
  `name` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `trains`
--

INSERT INTO `trains` (`id`, `user_id`, `name`) VALUES
(1, 1, 'V43'),
(2, 1, 'Bzmot');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `train_details`
--

CREATE TABLE `train_details` (
  `id` int(11) NOT NULL,
  `train_id` int(11) NOT NULL,
  `dcc_address` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `train_details`
--

INSERT INTO `train_details` (`id`, `train_id`, `dcc_address`) VALUES
(1, 1, 43),
(2, 2, 18);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `username` varchar(50) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `remember_token` varchar(255) DEFAULT NULL,
  `email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- A tábla adatainak kiíratása `users`
--

INSERT INTO `users` (`id`, `username`, `password_hash`, `remember_token`, `email`) VALUES
(1, 'Alex', 'znLhD5OqArQmHKxmFYUZ9Q==:J6PkBXpycIFVifne/7S1kMqpYwXUJkgJP1rr/lBx1P8=', 'eE36L1Nl5JpIzLnUBeJexzEGfklWu6p/decPIw3kY8Y=', '');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `functions`
--
ALTER TABLE `functions`
  ADD PRIMARY KEY (`id`),
  ADD KEY `train_id` (`train_id`);

--
-- A tábla indexei `functions_settings`
--
ALTER TABLE `functions_settings`
  ADD PRIMARY KEY (`id`),
  ADD KEY `function_id` (`function_id`);

--
-- A tábla indexei `trains`
--
ALTER TABLE `trains`
  ADD PRIMARY KEY (`id`),
  ADD KEY `user_id` (`user_id`);

--
-- A tábla indexei `train_details`
--
ALTER TABLE `train_details`
  ADD PRIMARY KEY (`id`),
  ADD KEY `train_id` (`train_id`);

--
-- A tábla indexei `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `username` (`username`),
  ADD UNIQUE KEY `email` (`email`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `functions`
--
ALTER TABLE `functions`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=59;

--
-- AUTO_INCREMENT a táblához `functions_settings`
--
ALTER TABLE `functions_settings`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=59;

--
-- AUTO_INCREMENT a táblához `trains`
--
ALTER TABLE `trains`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `train_details`
--
ALTER TABLE `train_details`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT a táblához `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `functions`
--
ALTER TABLE `functions`
  ADD CONSTRAINT `functions_ibfk_1` FOREIGN KEY (`train_id`) REFERENCES `trains` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `functions_settings`
--
ALTER TABLE `functions_settings`
  ADD CONSTRAINT `functions_settings_ibfk_2` FOREIGN KEY (`function_id`) REFERENCES `functions` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `trains`
--
ALTER TABLE `trains`
  ADD CONSTRAINT `trains_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`) ON DELETE CASCADE;

--
-- Megkötések a táblához `train_details`
--
ALTER TABLE `train_details`
  ADD CONSTRAINT `train_details_ibfk_1` FOREIGN KEY (`train_id`) REFERENCES `trains` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
