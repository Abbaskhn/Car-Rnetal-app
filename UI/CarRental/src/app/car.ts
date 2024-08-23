import { Binary } from "@angular/compiler";

export interface Car {
  carId: number;
  vendorId: number;
  model: number;
  carName: string;
  rentalprice: number;
  isAvailable: boolean;
  carFiles: {
    $id: string;
    $values: CarFile[];
  }; // Array of CarFile objects
}

export interface CarFile {
  id: number;
  carId: number;
  appFileId: number;
  carAppFiles: AppFile; // Reference to the file details
}

export interface AppFile {
  id: number;
  fileName: string;
  contentType: string;
  fileSize: number;
  data: string; // Base64 encoded image data
  uploadedOn: string;
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
