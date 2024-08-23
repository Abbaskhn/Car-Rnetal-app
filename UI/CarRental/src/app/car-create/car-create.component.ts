import { Component } from '@angular/core';
import { CarService } from '../car.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import {Car } from '../car';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { FileService } from '../services/file.service';
import { AppResponseModel } from '../auth.service';

@Component({
  selector: 'app-car-create',
  standalone: true,
  imports: [FormsModule,RouterLink,ReactiveFormsModule],
  templateUrl: './car-create.component.html',
  styleUrl: './car-create.component.css'
})
export class CarCreateComponent {
  carForm: FormGroup;
  carId: number | null = null;
  selectedFile: File | null = null;
  isUpdateMode: boolean = false;
 // fileId: number=0;
  constructor(
    private fb: FormBuilder,
    private carService: CarService,
    private route: ActivatedRoute,
    private fileSrv: FileService,
    private router: Router
  ) {
    this.carForm = this.fb.group({
      carName: ['', Validators.required],
      model: ['', Validators.required],
      rentalprice: ['', Validators.required],
      isAvailable: [false],
      fileId:[0]
    });

    const id = this.route.snapshot.params['id'];
    this.carId = id ? +id : null; // Convert id to a number or null
    this.isUpdateMode = !!this.carId; // Set update mode based on whether carId is present
  }

  ngOnInit(): void {
    if (this.isUpdateMode) {
      this.loadCarDetails();
    }
  }

  loadCarDetails(): void {
    if (this.carId) {
      this.carService.getCar(this.carId).subscribe((car: Car) => {
        this.carForm.patchValue({
          carName: car.carName,
          model: car.model,
          rentalprice: car.rentalprice,
          
        });
      });
    }
  }

  onFileChange(event: any): void {
    if (event.target.files.length > 0) {
      const formData = new FormData();
      formData.append('File', event.target.files[0]);
      this.fileSrv.save(formData).subscribe((response: AppResponseModel<number>) =>
      this.carForm.patchValue({
        fileId: response.data // set the new value for fileId
      })
      );
    }
  }

  onSubmit(): void {
    if (this.carForm.invalid) {
      return;
    }

    const formData = new FormData();
    formData.append('CarName', this.carForm.get('carName')?.value);
    formData.append('Model', this.carForm.get('model')?.value);
    formData.append('Rentalprice', this.carForm.get('rentalprice')?.value);
    

    if (this.isUpdateMode && this.carId) {
      // Update existing car
      this.carService.updateCar(this.carId, formData).subscribe(
        () => {
          alert('Car updated successfully!');
          this.router.navigate(['/car']);
        },
        error => {
          console.error('Error updating car:', error);
          alert('Failed to update car.');
        }
      );
    } else {
      // Create new car
      this.carService.createCar( this.carForm.value).subscribe(
        () => {
          alert('Car created successfully!');
          this.router.navigate(['/car']);
        },
        error => {
          console.error('Error creating car:', error);
          alert('Failed to create car.');
        }
      );
    }
  }
}