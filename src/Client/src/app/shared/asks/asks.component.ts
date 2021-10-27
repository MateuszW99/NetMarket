import { Component, OnInit } from '@angular/core';
import { AsksService } from "./asks.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ItemDetails } from "../items/item-details/item-details.model";
import { ToastrService } from "ngx-toastr";
import { Router } from "@angular/router";

@Component({
  selector: 'app-asks',
  templateUrl: './asks.component.html',
  styleUrls: ['./asks.component.css']
})
export class AsksComponent implements OnInit {

  itemDetails: ItemDetails;
  size: string;
  form: FormGroup;
  userWantsToPlaceAsk: boolean;

  constructor(
    private askService: AsksService,
    private toastrSerivce: ToastrService,
    private router: Router) { }

  ngOnInit(): void {
    // TODO: act in case data.X is empty/null
    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;

    this.form = new FormGroup({
      item: new FormControl(this.itemDetails.item.id, Validators.nullValidator),
      size: new FormControl(this.size, Validators.nullValidator),
      price: new FormControl('', [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')]), // TODO: offer newLowestAsk
    });

    this.userWantsToPlaceAsk = this.form.get('price').value >= this.itemDetails.lowestAsk.price;
  }

  isNewLowestAsk(): boolean {
    return this.form.get('price').value <= this.itemDetails.lowestAsk.price;
  }

  getLabel() {
    return this.userWantsToPlaceAsk ? 'Place Ask' : 'Sell';
  }

  onSubmitForm(): void {
    if (this.userWantsToPlaceAsk) {
      this.onPlaceAsk();
    }
    else {
      this.onSellNow();
    }
  }

  onPlaceAsk(): void {
    this.askService.placeAsk(this.form.value.item, this.form.value.size, this.form.value.price.toString())
      .subscribe(() => {
        this.router.navigate([`/items/${this.itemDetails.item.id}`])
          .then(() => this.toastrSerivce.success('Ask placed!'));
      });
  }

  onSellNow(): void {

  }
}
