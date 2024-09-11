import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { MaterialModule } from '../MaterailModule/MaterialModule';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MaterialModule,FormsModule,ReactiveFormsModule,RouterLink,RouterLinkActive],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent  implements OnInit {
  loginForm!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService, // Renamed from 'http' to 'authService' for clarity
    private router: Router
  ) {}

  ngOnInit(): void {
    this.createForm();
  }

  createForm(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onLogin(): void {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe(
        (res) => {
          console.log('User Login:', res); 
          this.loginForm.reset();
          this.authService.storeToken(res.data); // Ensure res.token exists and is correct
          const roles = res.data.roles.$values;
          if (roles.includes('Vendor')) {          
            this.router.navigate(['/car']);
          } else {
            this.router.navigate(['/booking']);
          }
        },
        (err) => {
          console.error('Error:', err);
        }
      );
    } else {
      console.error('Form is not valid'); 
    }
  }
}