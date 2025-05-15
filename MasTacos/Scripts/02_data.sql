-- MySQL script to populate tables with mock data
-- For Ma's Tacos Restaurant Management System

USE mas_tacos;

-- Optional: Clear existing data if needed
-- DELETE FROM MenuItems;
-- DELETE FROM Customers;

-- Insert data into Customers table
INSERT INTO Customers (FirstName, LastName, Email, Phone, MarketingOptIn, JoinDate, LoyaltyPoints) VALUES
    ('Maria', 'Garcia', 'maria.garcia@email.com', '555-123-4567', TRUE, '2024-01-15', 120),
    ('John', 'Smith', 'john.smith@email.com', '555-234-5678', TRUE, '2024-01-20', 85),
    ('Emily', 'Johnson', 'emily.j@email.com', '555-345-6789', FALSE, '2024-02-03', 50),
    ('Miguel', 'Rodriguez', 'miguel.r@email.com', '555-456-7890', TRUE, '2024-02-10', 175),
    ('Sarah', 'Williams', 'sarah.w@email.com', '555-567-8901', TRUE, '2024-02-15', 95),
    ('David', 'Brown', 'david.b@email.com', '555-678-9012', FALSE, '2024-02-28', 30),
    ('Lisa', 'Miller', 'lisa.m@email.com', '555-789-0123', TRUE, '2024-03-05', 140),
    ('James', 'Davis', 'james.d@email.com', '555-890-1234', TRUE, '2024-03-12', 70),
    ('Jennifer', 'Martinez', 'jennifer.m@email.com', '555-901-2345', FALSE, '2024-03-20', 25),
    ('Robert', 'Hernandez', 'robert.h@email.com', '555-012-3456', TRUE, '2024-03-28', 110),
    ('Patricia', 'Lopez', 'patricia.l@email.com', '555-123-7890', TRUE, '2024-04-02', 60),
    ('Michael', 'Wilson', 'michael.w@email.com', '555-234-8901', FALSE, '2024-04-10', 40),
    ('Linda', 'Anderson', 'linda.a@email.com', '555-345-9012', TRUE, '2024-04-18', 130),
    ('William', 'Thomas', 'william.t@email.com', '555-456-0123', TRUE, '2024-04-25', 80),
    ('Elizabeth', 'Jackson', 'elizabeth.j@email.com', '555-567-1234', FALSE, '2024-05-03', 20),
    ('Richard', 'White', 'richard.w@email.com', '555-678-2345', TRUE, '2024-05-12', 105),
    ('Susan', 'Harris', 'susan.h@email.com', '555-789-3456', TRUE, '2024-05-20', 55),
    ('Joseph', 'Clark', 'joseph.c@email.com', '555-890-4567', FALSE, '2024-05-28', 35),
    ('Jessica', 'Lewis', 'jessica.l@email.com', '555-901-5678', TRUE, '2024-06-05', 125),
    ('Thomas', 'Robinson', 'thomas.r@email.com', '555-012-6789', TRUE, '2024-06-15', 75),
    ('Nancy', 'Walker', 'nancy.w@email.com', '555-123-7890', FALSE, '2024-06-22', 15),
    ('Christopher', 'Young', 'chris.y@email.com', '555-234-8901', TRUE, '2024-06-30', 95),
    ('Karen', 'Allen', 'karen.a@email.com', '555-345-9012', TRUE, '2024-07-08', 45),
    ('Daniel', 'King', 'daniel.k@email.com', '555-456-0123', FALSE, '2024-07-15', 25),
    ('Betty', 'Wright', 'betty.w@email.com', '555-567-1234', TRUE, '2024-07-23', 115),
    ('Matthew', 'Scott', 'matthew.s@email.com', '555-678-2345', TRUE, '2024-07-31', 65),
    ('Dorothy', 'Green', 'dorothy.g@email.com', '555-789-3456', FALSE, '2024-08-08', 10),
    ('Anthony', 'Baker', 'anthony.b@email.com', '555-890-4567', TRUE, '2024-08-16', 85),
    ('Sandra', 'Adams', 'sandra.a@email.com', '555-901-5678', TRUE, '2024-08-24', 40),
    ('Mark', 'Nelson', 'mark.n@email.com', '555-012-6789', FALSE, '2024-09-01', 30);

-- Insert data into MenuItems table with NULL image data
INSERT INTO MenuItems (Name, Description, Price, Category, IsActive, PopularityScore, ImageData, ImageMimeType) VALUES
    ('Carne Asada Taco', 'Grilled steak taco with cilantro and onions', 4.50, 'Tacos', TRUE, 95, NULL, NULL),
    ('Chicken Tinga Taco', 'Shredded chicken in chipotle sauce', 4.00, 'Tacos', TRUE, 85, NULL, NULL),
    ('Al Pastor Taco', 'Marinated pork with pineapple', 4.25, 'Tacos', TRUE, 90, NULL, NULL),
    ('Vegetarian Taco', 'Grilled vegetables with guacamole', 3.75, 'Tacos', TRUE, 70, NULL, NULL),
    ('Fish Taco', 'Battered fish with cabbage slaw and lime crema', 4.75, 'Tacos', TRUE, 80, NULL, NULL),
    ('Shrimp Taco', 'Grilled shrimp with mango salsa', 5.00, 'Tacos', TRUE, 75, NULL, NULL),
    ('Birria Taco', 'Slow-cooked beef with consomme for dipping', 5.50, 'Tacos', TRUE, 98, NULL, NULL),
    ('Chorizo Taco', 'Spicy Mexican sausage with potato', 4.25, 'Tacos', TRUE, 65, NULL, NULL),
    ('Bean and Cheese Burrito', 'Refried beans and melted cheese', 7.50, 'Burritos', TRUE, 60, NULL, NULL),
    ('Carne Asada Burrito', 'Grilled steak with rice, beans, and salsa', 9.50, 'Burritos', TRUE, 88, NULL, NULL),
    ('Chicken Burrito', 'Grilled chicken with rice, beans, and salsa', 8.50, 'Burritos', TRUE, 82, NULL, NULL),
    ('Veggie Burrito', 'Grilled vegetables with rice, beans, and guacamole', 7.50, 'Burritos', TRUE, 60, NULL, NULL),
    ('Super Burrito', 'Meat, rice, beans, cheese, sour cream, and guacamole', 10.50, 'Burritos', TRUE, 85, NULL, NULL),
    ('Nachos Supreme', 'Tortilla chips topped with meat, cheese, beans, and sour cream', 9.00, 'Appetizers', TRUE, 78, NULL, NULL),
    ('Guacamole and Chips', 'Fresh guacamole with house-made tortilla chips', 6.50, 'Appetizers', TRUE, 72, NULL, NULL),
    ('Queso Fundido', 'Melted cheese with chorizo and tortillas', 7.00, 'Appetizers', TRUE, 65, NULL, NULL),
    ('Quesadilla', 'Flour tortilla with melted cheese and choice of filling', 6.50, 'Appetizers', TRUE, 70, NULL, NULL),
    ('Mexican Rice', 'Traditional tomato-based rice', 2.50, 'Sides', TRUE, 55, NULL, NULL),
    ('Refried Beans', 'Pinto beans cooked and mashed with spices', 2.50, 'Sides', TRUE, 50, NULL, NULL),
    ('Elote', 'Mexican street corn with mayo, cheese, and chili powder', 3.50, 'Sides', TRUE, 75, NULL, NULL),
    ('Horchata', 'Sweet rice milk with cinnamon', 3.00, 'Beverages', TRUE, 72, NULL, NULL),
    ('Jamaica', 'Hibiscus tea', 3.00, 'Beverages', TRUE, 65, NULL, NULL),
    ('Mexican Coca-Cola', 'Made with real sugar', 3.50, 'Beverages', TRUE, 80, NULL, NULL),
    ('Jarritos', 'Mexican fruit soda, various flavors', 3.00, 'Beverages', TRUE, 68, NULL, NULL),
    ('Churros', 'Fried dough pastry with cinnamon sugar', 4.00, 'Desserts', TRUE, 82, NULL, NULL),
    ('Flan', 'Caramel custard', 4.50, 'Desserts', TRUE, 75, NULL, NULL),
    ('Tres Leches Cake', 'Sponge cake soaked in three kinds of milk', 5.00, 'Desserts', TRUE, 80, NULL, NULL),
    ('Sopapillas', 'Fried pastry with honey and cinnamon', 4.00, 'Desserts', TRUE, 70, NULL, NULL),
    ('Taco Salad', 'Salad in a crispy tortilla bowl with choice of meat', 8.50, 'Entrees', TRUE, 68, NULL, NULL),
    ('Enchiladas', 'Corn tortillas filled with meat, topped with sauce and cheese', 9.50, 'Entrees', TRUE, 78, NULL, NULL);
