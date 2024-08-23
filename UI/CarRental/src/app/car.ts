export interface Car {
  carId: number;
  model: number;
  carName: string;
  fileId: number;
  rentalprice: number;
  carImage: string;
  isAvailable: boolean;
}

export interface CarDTO {
  carName: string;
  model: number;
  rentalprice: number;
  imageFile: File; // File type for image
}

export interface CarUpdateDTO {
  carId: number;
  carName: string;
  model: number;
  rentalprice: number;
  imageFile?: File; // Optional file for update
  carImage?: string; // Existing image path, optional
  isAvailable?: boolean; // Optional availability
}
