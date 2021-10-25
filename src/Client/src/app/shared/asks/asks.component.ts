import { Component, OnInit } from '@angular/core';
import { Size } from "../size.model";
import { AsksService } from "./asks.service";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { ItemDetails } from "../items/item-details/item-details.model";

@Component({
  selector: 'app-asks',
  templateUrl: './asks.component.html',
  styleUrls: ['./asks.component.css']
})
export class AsksComponent implements OnInit {

  itemDetails: ItemDetails;
  size: Size;
  form: FormGroup;
  userWantsToPlaceAsk: boolean;

  constructor(private askService: AsksService) { }

  ngOnInit(): void {
    // TODO: act in case data.X is empty/null
    this.itemDetails = history.state.data.item;
    this.size = history.state.data.size;

    this.form = new FormGroup({
      item: new FormControl(this.itemDetails.item, Validators.nullValidator),
      size: new FormControl('', Validators.nullValidator),
      price: new FormControl(this.itemDetails.highestBid.price, [ Validators.required, Validators.pattern('^[0-9]+(.[0-9]{0,2})?$')]),
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

  }

  onSellNow(): void {

  }
}
