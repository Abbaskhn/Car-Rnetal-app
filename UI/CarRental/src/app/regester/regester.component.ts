import { Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MaterialModule } from '../MaterailModule/MaterialModule';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-regester',
  standalone: true,
  imports: [FormsModule,ReactiveFormsModule,MaterialModule,RouterLink,RouterLinkActive],
  templateUrl: './regester.component.html',
  styleUrl: './regester.component.scss'
})
export class RegesterComponent implements OnInit {
  
    registrationForm!: FormGroup;
   
    constructor(private fb: FormBuilder, private http:AuthService, private router: Router) { }
  
    ngOnInit(): void {
      this.createForm();
    }
  
    createForm(): void {
      this.registrationForm = this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        username: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required]],
        roleId: ['', Validators.required],
      });
    }
  
    onSubmit(): void {
      if (this.registrationForm) {
        this.http.register(this.registrationForm.value).subscribe({
          next: (res) => {
            console.log('Response:', res);
            alert(res.message); // corrected spelling of "message"
            this.registrationForm.reset();
            this.router.navigate(['/login']);
          },
          error: (err) => {
            console.error('Error:', err);
            // Handle error (e.g., show error message)
          }
        });
      }
    }}