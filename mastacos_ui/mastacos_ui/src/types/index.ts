// Types for Ma's Tacos Restaurant Management System

// Menu Item interface
export interface MenuItem {
  id: number;
  name: string;
  description: string;
  price: number;
  category: string;
  isActive: boolean;
  popularityScore: number;
}

// Customer interface
export interface Customer {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  marketingOptIn: boolean;
  joinDate: string;
  loyaltyPoints: number;
}

// TimeSlot interface
export interface TimeSlot {
  id: number;
  startTime: string;
  endTime: string;
  dayOfWeek: string;
  avgCustomerVolume: number;
  peakHours: boolean;
}

// Reservation interface
export interface Reservation {
  id: number;
  customerId: number;
  reservationTime: string;
  partySize: number;
  status: 'Confirmed' | 'Completed' | 'Cancelled' | 'No-Show';
  specialRequests: string | null;
  timeSlotId: number;
}

// Order interface
export interface Order {
  id: number;
  customerId: number;
  orderTime: string;
  totalAmount: number;
  orderStatus: 'Pending' | 'Preparing' | 'Completed';
  orderType: 'Dine-In' | 'Takeout' | 'Delivery';
  timeSlotId: number | null;
}

// OrderItem interface
export interface OrderItem {
  id: number;
  orderId: number;
  menuItemId: number;
  quantity: number;
  unitPrice: number;
  specialInstructions: string | null;
}

// SurveyResponse interface
export interface SurveyResponse {
  id: number;
  customerId: number;
  orderId: number;
  submissionDate: string;
  foodRating: number;
  serviceRating: number;
  ambienceRating: number | null;
  feedback: string;
  followedUp: boolean;
}