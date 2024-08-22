export interface BookingCarDto {
    id: number; 
    carId: number;
    name: string;
    email: string;
    address: string;
    startDate: Date;
    endDate: Date;
    totalAmount?: number;
  }