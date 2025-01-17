import { Pipe, PipeTransform } from '@angular/core';
import { GetScopeDataDto } from '../models/scopes/getScopeDataDto';

@Pipe({
  name: 'filterByTimeLabel'
})
export class FilterByTimeLabelPipe implements PipeTransform {
  transform(value: GetScopeDataDto[], label: string): any[] {
    if (!value || !label) {
      return value;
    }
    return value.filter(item => item.timeDimension.timeAggregationLabel === label);
  }
}
