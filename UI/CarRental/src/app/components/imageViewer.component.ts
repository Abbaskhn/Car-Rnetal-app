import { NgIf } from '@angular/common';
import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'app-image-viewer',
  standalone: true,
  imports: [NgIf],
  template: `
    <div *ngIf="imageSrc; else noImage">
      <img [src]="imageSrc" alt="Loaded Image" />
    </div>
    <ng-template #noImage>
      <p>No image available.</p>
    </ng-template>
  `,
})
export class ImageViewerComponent implements OnChanges {
  @Input() ImageData: string | null = null;
  imageSrc: string | null = null;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['ImageData'] && this.ImageData) {
      this.createImageFromBinary(this.ImageData);
    }
  }

  createImageFromBinary(base64Data: string): void {
    this.imageSrc = `data:image/jpeg;base64,${base64Data}`;
    // Adjust MIME type if needed, based on your image data.
  }
}