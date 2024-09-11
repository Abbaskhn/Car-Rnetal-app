import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegesterComponent } from './regester/regester.component';
import { DashboardComponent} from './dashboard/dashboard.component';
import { VendorComponent } from './vendor/vendor.component';
import { CustomerComponent } from './customer/customer.component';
import { CarComponent } from './car/car.component';
import { CarCreateComponent } from './car-create/car-create.component';
import { BookingComponent } from './booking/booking.component';
import { BookinglistComponentt } from './bookinglist/bookinglist.component';
import { CheackoutComponent } from './cheackout/cheackout.component';
import { LayoutComponent } from './layout/layout.component';
import { CustomerlistComponent } from './customerlist/customerlist.component';
import {  VendorListComponent } from './vendor-list/vendor-list.component';

export const routes: Routes = [
    { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegesterComponent },
    { path: 'dashboard', component: DashboardComponent },
    { path: 'vendor', component: VendorComponent },
    { path: 'customer', component: CustomerComponent },
    {
        path: '', 
        component: LayoutComponent, 
        children: [
            { path: 'vendorList', component: VendorListComponent },
          
            { path: 'customerlist', component:CustomerlistComponent },
            { path: 'car', component: CarComponent },
            { path: 'createcar', component: CarCreateComponent },
            { path: 'createcar/:id', component: CarCreateComponent },
            { path: 'booking', component: BookingComponent },
            { path: 'book/:carId', component: BookingComponent },
            { path: 'bookinglist', component: BookinglistComponentt },
            { path: 'checkout', component: CheackoutComponent }
        ]
    }
];