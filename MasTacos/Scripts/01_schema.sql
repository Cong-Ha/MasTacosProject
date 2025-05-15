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


-- Add indexes for performance
CREATE INDEX idx_customer_email ON Customers(Email);

