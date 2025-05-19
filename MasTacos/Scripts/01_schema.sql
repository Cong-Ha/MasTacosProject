-- MySQL script to create tables based on the restaurant management ERD
-- Creates tables if they don't exist in the specified schema

CREATE SCHEMA IF NOT EXISTS mas_tacos;
USE mas_tacos;

-- Create Customers table
CREATE TABLE IF NOT EXISTS Customers (
    CustomerId INT AUTO_INCREMENT PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(100) UNIQUE,
    Phone VARCHAR(20),
    MarketingOptIn BOOLEAN DEFAULT FALSE,
    JoinDate DATE NOT NULL,
    LoyaltyPoints INT DEFAULT 0
);

-- Create MenuItems table
CREATE TABLE IF NOT EXISTS MenuItems (
    ItemId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    PopularityScore INT DEFAULT 0,
    ImageData LONGBLOB NULL,
    ImageMimeType VARCHAR(100) NULL
);

-- AspNetUsers table
CREATE TABLE IF NOT EXISTS `AspNetUsers` (
    `Id` VARCHAR(255) NOT NULL,
    `UserName` VARCHAR(256) NULL,
    `FirstName` VARCHAR(100) NULL,
    `LastName` VARCHAR(100) NULL,
    `NormalizedUserName` VARCHAR(256) NULL,
    `Email` VARCHAR(256) NULL,
    `NormalizedEmail` VARCHAR(256) NULL,
    `EmailConfirmed` TINYINT(1) NOT NULL,
    `PasswordHash` TEXT NULL,
    `SecurityStamp` TEXT NULL,
    `ConcurrencyStamp` TEXT NULL,
    `PhoneNumber` TEXT NULL,
    `PhoneNumberConfirmed` TINYINT(1) NOT NULL,
    `TwoFactorEnabled` TINYINT(1) NOT NULL,
    `LockoutEnd` DATETIME NULL,
    `LockoutEnabled` TINYINT(1) NOT NULL,
    `AccessFailedCount` INT NOT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE INDEX `UserNameIndex` (`NormalizedUserName`),
    INDEX `EmailIndex` (`NormalizedEmail`)
    );

-- AspNetRoles table
CREATE TABLE IF NOT EXISTS `AspNetRoles` (
    `Id` VARCHAR(255) NOT NULL,
    `Name` VARCHAR(256) NULL,
    `NormalizedName` VARCHAR(256) NULL,
    `ConcurrencyStamp` TEXT NULL,
    PRIMARY KEY (`Id`),
    UNIQUE INDEX `RoleNameIndex` (`NormalizedName`)
    );

-- AspNetUserRoles table
CREATE TABLE IF NOT EXISTS `AspNetUserRoles` (
    `UserId` VARCHAR(255) NOT NULL,
    `RoleId` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    INDEX `IX_AspNetUserRoles_RoleId` (`RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
    );

-- AspNetUserClaims table
CREATE TABLE IF NOT EXISTS `AspNetUserClaims` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `UserId` VARCHAR(255) NOT NULL,
    `ClaimType` TEXT NULL,
    `ClaimValue` TEXT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_AspNetUserClaims_UserId` (`UserId`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    );

-- AspNetUserLogins table
CREATE TABLE IF NOT EXISTS `AspNetUserLogins` (
    `LoginProvider` VARCHAR(128) NOT NULL,
    `ProviderKey` VARCHAR(128) NOT NULL,
    `ProviderDisplayName` TEXT NULL,
    `UserId` VARCHAR(255) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    INDEX `IX_AspNetUserLogins_UserId` (`UserId`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    );

-- AspNetRoleClaims table
CREATE TABLE IF NOT EXISTS `AspNetRoleClaims` (
    `Id` INT NOT NULL AUTO_INCREMENT,
    `RoleId` VARCHAR(255) NOT NULL,
    `ClaimType` TEXT NULL,
    `ClaimValue` TEXT NULL,
    PRIMARY KEY (`Id`),
    INDEX `IX_AspNetRoleClaims_RoleId` (`RoleId`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
    );

-- AspNetUserTokens table
CREATE TABLE IF NOT EXISTS `AspNetUserTokens` (
    `UserId` VARCHAR(255) NOT NULL,
    `LoginProvider` VARCHAR(128) NOT NULL,
    `Name` VARCHAR(128) NOT NULL,
    `Value` TEXT NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
    );

-- Add indexes for performance
CREATE INDEX idx_customer_email ON Customers(Email);

