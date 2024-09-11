import { Component, OnInit } from '@angular/core';
import { CustomerService } from '../customer.service';
import { Customer } from '../customer';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [FormsModule,ReactiveFormsModule,FormsModule,NgIf],
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent  implements OnInit {
  registerForm: FormGroup;
  isSubmitting = false;
  errorMessage: string | null = null;

  roles = [
    { value: 'Customer', display: 'Customer' },
    { value: 'Vendor', display: 'Vendor' }
  ];

  constructor(private fb: FormBuilder, private userService: CustomerService) {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      address: ['', Validators.required],
      phone: ['', [Validators.required, Validators.pattern('^[0-9]+$')]],
      userName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      role: ['Customer', Validators.required] // Default to 'Customer'
    });
  }
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  onSubmit(): void {
    if (this.registerForm.invalid) {
      return;
    }

    this.isSubmitting = true;
    this.userService.postData(this.registerForm.value).subscribe(
      response => {
        if (response.success) {
          // Handle successful registration
          console.log('Registration successful:', response);
          // Redirect or show success message
        } else {
          // Handle registration failure
          this.errorMessage = response.message;
          console.error('Registration failed:', response);
        }
        this.isSubmitting = false;
      },
      error => {
        this.errorMessage = 'An error occurred during registration.';
        console.error('Registration error:', error);
        this.isSubmitting = false;
      }
    );
  }
}