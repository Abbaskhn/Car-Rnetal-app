import { NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { StripeService } from '../stripe.service';
import { loadStripe, Stripe, StripeCardElement, StripeElements } from '@stripe/stripe-js';
import { PaymentSuccessComponent } from '../payment-success/payment-success.component';

@Component({
  selector: 'app-cheackout',
  standalone: true,
  imports: [NgIf],
  templateUrl: './cheackout.component.html',
  styleUrl: './cheackout.component.css'
})
export class CheackoutComponent implements OnInit {
  stripe: Stripe | null = null;
  elements: StripeElements | null = null;
  card: StripeCardElement | null = null;
  errorMessage: string = '';

  constructor(private stripeService: StripeService, private dialog: MatDialog) {}

  async ngOnInit(): Promise<void> {
    this.stripe = await loadStripe('pk_test_51PodXrP2w7G5EZN3yWTbWcac7uMD4hFbw5XDV4j1cKpTbEyR2XdvsI0aRCVhnswe982ril5OgcYW2zIjjQMACv8Q00aqPtJopc');
    if (!this.stripe) {
      this.errorMessage = 'Stripe.js failed to load.';
      return;
    }

    this.elements = this.stripe.elements();
    this.card = this.elements.create('card');
    this.card.mount('#card-element');
  }

  async checkout(event: Event): Promise<void> {
    event.preventDefault();

    if (!this.stripe || !this.card) {
      this.errorMessage = 'Stripe or card element not loaded.';
      return;
    }

    const cardElement = this.card;
    const { token, error } = await this.stripe.createToken(cardElement);

    if (error) {
      this.errorMessage = error.message || 'An error occurred while creating the token.';
      return;
    }

    try {
      const response = await this.stripeService.charge(
        token!.id,
        (document.getElementById('name') as HTMLInputElement).value,
        (document.getElementById('email') as HTMLInputElement).value,
        5000, // Amount in cents
        'Sample Charge'
      ).toPromise();

      console.log('Charge successful', response);

      // Open the success dialog
      this.dialog.open(PaymentSuccessComponent);
    } catch (err) {
      console.error('Charge failed', err);
      this.errorMessage = 'An error occurred while processing the payment.';
    }
  }
}