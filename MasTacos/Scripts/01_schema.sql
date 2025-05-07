-- MySQL script to create tables based on the restaurant management ERD
-- Creates tables if they don't exist in the specified schema

-- Create schema if it doesn't exist
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

-- Create TimeSlots table
CREATE TABLE IF NOT EXISTS TimeSlots (
    SlotId INT AUTO_INCREMENT PRIMARY KEY,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL,
    DayOfWeek ENUM('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'),
    AvgCustomerVolume INT,
    PeakHours BOOLEAN DEFAULT FALSE
);

-- Create Reservations table
CREATE TABLE IF NOT EXISTS Reservations (
    ReservationId INT AUTO_INCREMENT PRIMARY KEY,
    CustomerId INT NOT NULL,
    ReservationTime DATETIME NOT NULL,
    PartySize INT NOT NULL,
    Status ENUM('Confirmed', 'Cancelled', 'Completed', 'No-Show') DEFAULT 'Confirmed',
    SpecialRequests TEXT,
    TimeSlotId INT,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId) ON DELETE CASCADE,
    FOREIGN KEY (TimeSlotId) REFERENCES TimeSlots(SlotId) ON DELETE SET NULL
);

-- Create MenuItems table
CREATE TABLE IF NOT EXISTS MenuItems (
    ItemId INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Description TEXT,
    Price DECIMAL(10, 2) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    IsActive BOOLEAN DEFAULT TRUE,
    PopularityScore INT DEFAULT 0
);

-- Create Orders table
CREATE TABLE IF NOT EXISTS Orders (
    OrderId INT AUTO_INCREMENT PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderTime DATETIME NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    OrderStatus ENUM('Pending', 'Preparing', 'Ready', 'Delivered', 'Completed', 'Cancelled') DEFAULT 'Pending',
    OrderType ENUM('Dine-In', 'Takeout', 'Delivery') NOT NULL,
    TimeSlotId INT,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId) ON DELETE CASCADE,
    FOREIGN KEY (TimeSlotId) REFERENCES TimeSlots(SlotId) ON DELETE SET NULL
);

-- Create OrderItems table (junction table between Orders and MenuItems)
CREATE TABLE IF NOT EXISTS OrderItems (
    OrderItemId INT AUTO_INCREMENT PRIMARY KEY,
    OrderId INT NOT NULL,
    MenuItemId INT NOT NULL,
    Quantity INT NOT NULL DEFAULT 1,
    UnitPrice DECIMAL(10, 2) NOT NULL,
    SpecialInstructions TEXT,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(ItemId) ON DELETE RESTRICT
);

-- Create SurveyResponses table
CREATE TABLE IF NOT EXISTS SurveyResponses (
    ResponseId INT AUTO_INCREMENT PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderId INT NOT NULL,
    SubmissionDate DATETIME NOT NULL,
    FoodRating INT CHECK (FoodRating BETWEEN 1 AND 5),
    ServiceRating INT CHECK (ServiceRating BETWEEN 1 AND 5),
    AmbienceRating INT CHECK (AmbienceRating BETWEEN 1 AND 5),
    Feedback TEXT,
    FollowedUp BOOLEAN DEFAULT FALSE,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId) ON DELETE CASCADE,
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId) ON DELETE CASCADE,
    CONSTRAINT one_survey_per_order UNIQUE (OrderId)
);

-- Add indexes for performance
CREATE INDEX idx_customer_email ON Customers(Email);
CREATE INDEX idx_orders_customer ON Orders(CustomerId);
CREATE INDEX idx_orderitems_order ON OrderItems(OrderId);
CREATE INDEX idx_orderitems_menuitem ON OrderItems(MenuItemId);
CREATE INDEX idx_reservations_customer ON Reservations(CustomerId);
CREATE INDEX idx_reservations_time ON Reservations(ReservationTime);
CREATE INDEX idx_timeslots_day_start ON TimeSlots(DayOfWeek, StartTime);