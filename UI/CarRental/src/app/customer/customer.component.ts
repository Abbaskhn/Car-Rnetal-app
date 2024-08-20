import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../customer.service';
import { Customer } from '../customer';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [FormsModule,ReactiveFormsModule],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent  implements OnInit {
  customerForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private customerService: CustomerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    this.customerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      address: ['']
    });
  }

  onSubmit(): void {
    if (this.customerForm.valid) {
      const formData: Customer = this.customerForm.value;
      this.customerService.postData(formData).subscribe(
        response => {
          console.log('Data submitted successfully:', response);
          this.router.navigate(['/car']);
        },
        error => {
          console.error('Error submitting data:', error);
        }
      );
    } else {
      console.log('Form is not valid');
    }
  }
}