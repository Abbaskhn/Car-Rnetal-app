import { Component, OnInit } from '@angular/core';

import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from '../auth.service';
import { MaterialModule } from '../MaterailModule/MaterialModule';


@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [MaterialModule, RouterOutlet, ],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.scss'
})
export class LayoutComponent implements OnInit{


  sidenav = false;
  constructor(private router:Router ,private http:AuthService){}
  ngOnInit(): void {
    // Initial check for token expiration when the app starts
  
  
    this.http.checkTokenExpiration();
    
  }
  logout(){
    this.http.logout();
    alert("User Logout")
    
  }
  toggle(): void {
    this.sidenav = !this.sidenav;
  }

 
  Car(){
    this.router.navigate(['/car']);
  }
  
  Bookinglist(){
    this.router.navigate(['/bookinglist']);
  }
  Customer(){
    this.router.navigate(['/customerlist']);
  }
  Vendor(){
    this.router.navigate(['/vendorList']);
  }
 
 
}
