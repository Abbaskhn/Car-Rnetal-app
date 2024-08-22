import { APP_ID, Component } from '@angular/core';
import { VendorService } from '../vendor.service';
import { Vendor } from '../vendor';
import { Router, RouterLink, RouterLinkActive, RouterModule } from '@angular/router';
import { FormGroup, FormsModule } from '@angular/forms';


@Component({
  selector: 'app-vendor',
  standalone: true,
  imports: [RouterLinkActive,RouterLink,FormsModule,RouterModule],
  templateUrl: './vendor.component.html',
  styleUrl: './vendor.component.css'
})
export class VendorComponent {
  data: Vendor[] = []; // Define data with type Vendor[]
  vendor!: FormGroup;
  isAdmin = false;
  isVendor = false;
  isCustomer = false;

  constructor(private vendorService: VendorService, private router: Router) {}

  
  
    formData = {
      name: '',
      email: '',
      phone: '',
      company: ''
    };
    
  
    onSubmit() {
      // Validate the form data (optional)
      if (this.formData.name && this.formData.email && this.formData.phone) {
        // Send the data to the service
        this.vendorService.saveFormData(this.formData)
          .subscribe(response => {
            console.log('Form data saved successfully', response);
            this.router.navigate(['/car']);
          }, error => {
            console.error('Error saving form data', error);
          });
      } else {
        console.log('Form is not valid');
      }
 
    }
}