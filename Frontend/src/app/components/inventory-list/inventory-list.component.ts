import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { InventoryService } from '../../services/inventory.service';

@Component({
  selector: 'app-inventory-list',
  templateUrl: './inventory-list.component.html',
  styleUrls: ['./inventory-list.component.css']
})

export class InventoryListComponent implements OnInit {
  inventoryItems: any[] = [];
  currentPage: number = 1;
  pageSize: number = 5;
  pageCount: number = 1;
  searchTerm: string = '';
  orderBy: string = '';

  constructor(public inventoryService: InventoryService, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.loadItems();
  }

  loadItems(searchTerm: string = '') {
    const params = {
      pageNumber: this.currentPage,
      pageSize: this.pageSize,
      orderBy: this.orderBy,
      searchTerm: searchTerm
    };
    this.inventoryService.getInventoryItems(params).subscribe({
      next: (data) => {
        this.inventoryItems = data.result;
        this.currentPage = data.currentPage;
        this.pageCount = data.pageCount;
      },
      error: (error) => {
        console.error('Failed to load items', error);
        alert('Error loading items');
      }
    });
  }

  onSearch() {
    this.currentPage = 1;
    this.loadItems(this.searchTerm);
  }

  deleteItem(id: string) {
    this.inventoryService.deleteInventoryItem(id).subscribe({
      next: () => {
        this.toastr.success('Item deleted successfully', 'Deleted');

        this.loadItems(this.searchTerm);
      },
      error: (error) => {
        console.error('Error deleting item', error);
        this.toastr.error('Failed to delete item', 'Error');
      }
    });
  }


  editItem(item: any) {
  }

  newItem() {
  }

  sort(property: string) {
    this.orderBy = property;
    this.loadItems(this.searchTerm);
  }

  changePage(newPage: number) {
    if (newPage < 1 || newPage > this.pageCount) {
      return;
    }
    this.currentPage = newPage;
    this.loadItems(this.searchTerm);
  }
}
