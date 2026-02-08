export interface ParkingLot {
  id: string;
  name: string;
  address: string;
  firstHourAmount: number;
  extraHourAmount: number;
  lotsAvaliable: number;
  userId: string;

  isActive: boolean;
}
