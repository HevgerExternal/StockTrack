import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { InventoryService } from '../../services/inventory.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-inventory-item-form',
  templateUrl: './inventory-item-form.component.html',
  styleUrls: ['./inventory-item-form.component.css']
})
export class InventoryItemFormComponent implements OnInit {
  item: any = {
    name: '',
    itemNumber: '',
    location: '',
    quantity: 0
  };
  isEditMode: boolean = false;
  errors: any = {};

  constructor(
    private route: ActivatedRoute,
    private inventoryService: InventoryService,
    private toastr: ToastrService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const itemId = params.get('id');
      if (itemId) {
        this.isEditMode = true;
        this.loadItem(itemId);
      }
    });
  }

  loadItem(id: string) {
    this.inventoryService.getInventoryItemById(id).subscribe(item => {
      this.item = item;
    }, error => {
      this.toastr.error('Failed to load item', 'Error');
    });
  }

  onSubmit() {
    if (this.isEditMode) {
      this.updateItem();
    } else {
      this.addItem();
    }
  }

  addItem() {
    this.inventoryService.addInventoryItem(this.item).subscribe({
      next: () => {
        this.toastr.success('Item added successfully', 'Success');
        this.router.navigate(['/inventory']);
      },
      error: (errorResponse) => {
        if (errorResponse.error && typeof errorResponse.error === 'string') {
          this.toastr.error(errorResponse.error, 'Validation Error');  // Display specific error message
        }

        else if (errorResponse.error.errors) {
          this.errors = errorResponse.error.errors;
          this.toastr.error('Please fix the validation errors', 'Validation Error');
        }

        else {
          this.toastr.error('Bad Request', 'Validation Error');
        }
      }
    });
  }


  updateItem() {
    this.inventoryService.updateInventoryItem(this.item.id, this.item).subscribe({
      next: () => {
        this.toastr.success('Item updated successfully', 'Success');
        this.router.navigate(['/inventory']);
      },
      error: (errorResponse) => {
        if (errorResponse.status === 400 && errorResponse.error.errors) {
          this.errors = errorResponse.error.errors;
          this.toastr.error('Please fix the validation errors', 'Validation Error');
        } else {
          this.toastr.error('Failed to update item', 'Error');
        }
      }
    });
  }

}
