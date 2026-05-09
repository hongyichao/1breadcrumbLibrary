import React from 'react';

interface Props {
  search: string;
  availabilityFilter: boolean | undefined;
  onSearchChange: (value: string) => void;
  onAvailabilityChange: (value: boolean | undefined) => void;
}

export function SearchBar({ search, availabilityFilter, onSearchChange, onAvailabilityChange }: Props) {
  const handleAvailability = (e: React.ChangeEvent<HTMLSelectElement>) => {
    const val = e.target.value;
    if (val === 'true') onAvailabilityChange(true);
    else if (val === 'false') onAvailabilityChange(false);
    else onAvailabilityChange(undefined);
  };

  return (
    <div style={{ display: 'flex', gap: '12px', alignItems: 'center' }}>
      <input
        type="text"
        placeholder="Search by book title..."
        value={search}
        onChange={(e) => onSearchChange(e.target.value)}
        style={{ padding: '8px 12px', fontSize: '14px', border: '1px solid #ccc', borderRadius: '4px', width: '280px' }}
      />
      <select
        value={availabilityFilter === undefined ? '' : String(availabilityFilter)}
        onChange={handleAvailability}
        style={{ padding: '8px 12px', fontSize: '14px', border: '1px solid #ccc', borderRadius: '4px' }}
      >
        <option value="">All</option>
        <option value="true">Available</option>
        <option value="false">Unavailable</option>
      </select>
    </div>
  );
}
