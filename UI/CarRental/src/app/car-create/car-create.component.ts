import { Component } from '@angular/core';
import { CarService } from '../car.service';
import { Router, RouterLink } from '@angular/router';
import { Car } from '../car';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-car-create',
  standalone: true,
  imports: [FormsModule,RouterLink],
  templateUrl: './car-create.component.html',
  styleUrl: './car-create.component.css'
})
export class CarCreateComponent {
  car: Car = {
    carid: 0,
    carName: '',
    model: '',
    rentalprice: 0,
    carPhoto: '',
  };
  imageFile: File | null = null;

  constructor(private carService: CarService, private router: Router) {}

  onFileChange(event: any) {
    this.imageFile = event.target.files[0];
  }

  createCar() {
    if (this.imageFile) {
      this.carService.createCar(this.car, this.imageFile).subscribe(() => {
        this.router.navigate(['/car']);
      });
    }
  }
}