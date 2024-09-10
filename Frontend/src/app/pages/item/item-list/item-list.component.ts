import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { ItemService } from 'src/app/services/item.service';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { NzMessageService } from 'ng-zorro-antd/message';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.less']
})
export class ItemListComponent implements OnInit {
  total = 0;
  listOfItems: any[] = [];
  loading = true;
  pageSize = 10;
  pageIndex = 1;

  constructor(
    private itemService: ItemService,
    private location: Location,
    private message: NzMessageService // Inject NzMessageService
  ) {}

  ngOnInit(): void {
    this.loadDataFromServer(this.pageIndex, this.pageSize, null, null, []);
  }

  loadDataFromServer(
    pageIndex: number,
    pageSize: number,
    sortField: string | null,
    sortOrder: string | null,
    filters: Array<{ key: string; value: string[] }>
  ): void {
    this.loading = true;
    this.itemService.getItems(pageIndex, pageSize, sortField, sortOrder, filters).subscribe(response => {
      this.loading = false;
      this.total = response.totalCount;  // Set totalCount for pagination
      this.listOfItems = response.items;  // Set items array from the response
    }, error => {
      this.loading = false;
      this.message.error('Error loading items');
    });
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex, sort, filter } = params;
    const currentSort = sort.find(item => item.value !== null);
    const sortField = currentSort?.key || null;
    const sortOrder = currentSort?.value || null;
    this.loadDataFromServer(pageIndex, pageSize, sortField, sortOrder, filter);
  }

  deleteItem(item: any): void {
    this.itemService.deleteItem(item.itemId).subscribe(() => {
      this.listOfItems = this.listOfItems.filter(i => i.itemId !== item.itemId);
      this.message.success('Item deleted successfully');
    }, error => {
      this.message.error('Error deleting item');
    });
  }

  onBack(): void {
    this.location.back();
  }
}
