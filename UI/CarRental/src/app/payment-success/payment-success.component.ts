import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-payment-success',
  standalone: true,
  imports: [],
  templateUrl: './payment-success.component.html',
  styleUrl: './payment-success.component.css'
})
export class PaymentSuccessComponent {

  constructor(public dialogRef: MatDialogRef<PaymentSuccessComponent>) { }

  onClose(): void {
    this.dialogRef.close();
  }
}